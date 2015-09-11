namespace USO.Infrastructure.Activities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Data.Entity;
    using System.Linq;
    using USO.Domain;
    using USO.Domain.Extensions;
    using USO.Dto;
    using USO.Dto.Activities;
    using USO.Infrastructure.Mappers;
    using USO.Infrastructure.Security;

    public class ActivityService : IActivityService
    {
        private readonly IDatabaseContext _databaseContext;
        private readonly IActivityMapper _activityMapper;
        private readonly IActivityFileMapper _activityFileMapper;
        private readonly IOrganizationsGetter _organizationsGetter;
        private readonly IMembershipService _membershipService;

        public ActivityService(
            IDatabaseContext databaseContext,
            IActivityMapper activityMapper,
            IActivityFileMapper activityFileMapper,
            OrganizationsGetter organizationsGetter,
            IMembershipService membershipService
            )
        {
            _databaseContext = databaseContext;
            _activityMapper = activityMapper;
            _activityFileMapper = activityFileMapper;
            _organizationsGetter = organizationsGetter;
            _membershipService = membershipService;
        }

        public ServiceResult<ActivityDTO> CreateActivity(ActivityCreateParams activity)
        {
            Check.Argument.IsNotNull(activity, "activity");

            var result =
                Validation.Validate<ServiceResult<ActivityDTO>>(() => string.IsNullOrEmpty(activity.Title), "Title",
                                                                "标题不能为空")
                          .And(() => !activity.ActivityType.HasValue, "ActivityType", "必须选择任务类型")
                          .And(() => !activity.StartedOn.HasValue, "StartedOn", "开始时间不能为空")
                          .And(() => activity.OwningUserId <= 0, "OwningUserName", "创建者不能为空")
                          .Result();

            var allOrgName = new List<string>
                {
                    "ChangHong",
                    "ChangHongAll"
                };

            if (activity.ParentId.GetValueOrDefault(0) == 0)
            {
                var participant = activity.ParticipantNames.Split(';').ToList();
                if (participant.Count(a => !allOrgName.Contains(a)) == 0)
                {
                    result.RuleViolations.Add(new RuleViolation("participant", "发给所有人的帖子,Test............."));
                }
            }
            if (!result.RuleViolations.IsEmpty()) return result;

            var activityEntity = new Activity
                {
                    ParentId = activity.ParentId,
                    ActivityType = activity.ActivityType,
                    Priority = activity.Priority,
                    Title = activity.Title,
                    AssignedOn = activity.AssignedOn,
                    DateDue = activity.DateDue,
                    StartedOn = activity.StartedOn ?? DateTime.UtcNow,
                    CompletedOn = activity.CompletedOn,
                    ActualHours = activity.ActualHours,
                    Detail = activity.Detail,
                    EstimatedProgress = activity.EstimatedProgress,
                    OwningUserId = activity.OwningUserId,
                    OrganizationId = activity.OrganizationId,
                    Status = activity.Status,
                    CreatedBy = activity.OwningUserId,
                    ModifiedBy = activity.OwningUserId,
                    CreatedOn = DateTime.UtcNow,
                    ModifiedOn = DateTime.UtcNow,
                    ParticipantIds = activity.ParticipantIds,
                    ParticipantNames = activity.ParticipantNames,
                    ReAssigned = activity.ReAssigned
                };

            var orgParticipantIds = new List<int>();
            var userParticipantIds = new List<int>();
            if (!string.IsNullOrWhiteSpace(activity.ParticipantIds))
            {
                var ids = activity.ParticipantIds.Split(";".ToArray(), StringSplitOptions.RemoveEmptyEntries);
                if (ids.Length > 0)
                {
                    foreach (var id in ids)
                    {
                        var pv = id.Split(new[] {'_'}, StringSplitOptions.RemoveEmptyEntries);
                        if (pv.Length != 2)
                            continue;

                        if (pv[0].ToLower() == "U".ToLower())
                        {
                            int userId;
                            if (int.TryParse(pv[1], out userId))
                            {
                                if (!userParticipantIds.Contains(userId))
                                    userParticipantIds.Add(userId);
                            }
                        }
                        else if (pv[0].ToLower() == "O".ToLower())
                        {
                            int orgId;
                            if (int.TryParse(pv[1], out orgId))
                            {
                                if (!orgParticipantIds.Contains(orgId))
                                    orgParticipantIds.Add(orgId);
                            }
                        }
                    }
                }
            }

            if (!userParticipantIds.Contains(activityEntity.OwningUserId))
                userParticipantIds.Add(activityEntity.OwningUserId);

            //标记已读
            activityEntity.Reads.Add(new ActivityRead
                {
                    ActivityId = activityEntity.Id,
                    UserId = activityEntity.OwningUserId,
                    Read = true
                });

            foreach (var participantId in userParticipantIds)
            {
                activityEntity.Participants.Add(new ActivityParticipant
                    {
                        ActivityId = activityEntity.Id,
                        ParticipantId = participantId,
                        OperateType =
                            participantId != activityEntity.OwningUserId
                                ? ActivityParticipantOperateType.User
                                : ActivityParticipantOperateType.OwningUser
                    });
            }

            foreach (var participantId in orgParticipantIds)
            {
                activityEntity.Participants.Add(new ActivityParticipant
                    {
                        ActivityId = activityEntity.Id,
                        ParticipantId = participantId,
                        OperateType = ActivityParticipantOperateType.Organization
                    });
            }

            try
            {
                _databaseContext.Activities.Add(activityEntity);
                _databaseContext.SaveChanges();

                if (activity.ParentId.HasValue)
                {
                    var parentActivityEntity = _databaseContext.Activities
                                                               .Include(a => a.Reads)
                                                               .FirstOrDefault(a => a.Id == activity.ParentId.Value);
                    if (parentActivityEntity != null)
                    {
                        parentActivityEntity.Status = ActivityStatuses.Processing;
                        parentActivityEntity.ModifiedOn = DateTime.UtcNow;
                        parentActivityEntity.ModifiedBy = activity.OwningUserId;

                        // 新增回复则需要修改父活动的参与者状态
                        foreach (var read in parentActivityEntity.Reads)
                        {
                            if (read.UserId == activityEntity.OwningUserId)
                            {
                                read.Read = true;
                            }
                            else
                            {
                                read.Read = false;
                                read.Hidden = false;
                            }
                        }
                        _databaseContext.SaveChanges();
                    }
                }
            }
            catch (Exception ex)
            {
                result.RuleViolations.Add(new RuleViolation("_FORM", string.Format("不能保存活动记录。错误信息：{0}", ex.Message)));
                return result;
            }

            result = new ServiceResult<ActivityDTO>(_activityMapper.Map(activityEntity));
            return result;
        }

        public ServiceResult<ActivityDTO> UpdateActivity(ActivityEditParams activity)
        {
            Check.Argument.IsNotNull(activity, "activity");

            var result =
                Validation.Validate<ServiceResult<ActivityDTO>>(() => string.IsNullOrEmpty(activity.Title),
                                                                "activity.Title", "标题不能为空")
                          .And(() => !activity.ActivityType.HasValue, "ActivityType", "必须选择任务类型")
                          .And(() => !activity.StartedOn.HasValue, "StartedAt", "开始时间不能为空")
                          .And(() => activity.ModifiedBy <= 0, "ModifiedByUser", "修改者不能为空")
                          .Result();

            if (!result.RuleViolations.IsEmpty()) return result;

            var activityEntity = _databaseContext.Activities
                                                 .Include(a => a.Participants)
                                                 .Include(a => a.Reads)
                                                 .Include(a => a.Files)
                                                 .FirstOrDefault(a => a.Id == activity.Id);
            if (activityEntity == null)
            {
                result.RuleViolations.Add(new RuleViolation("_FORM", "找不到活动记录"));
                return result;
            }

            activityEntity.ActivityType = activity.ActivityType;

            activityEntity.Priority = activity.Priority;

            activityEntity.Title = activity.Title;
            activityEntity.AssignedOn = activity.AssignedOn;
            activityEntity.DateDue = activity.DateDue;
            activityEntity.StartedOn = activity.StartedOn ?? DateTime.UtcNow;
            activityEntity.CompletedOn = activity.CompletedOn;

            activityEntity.ActualHours = activity.ActualHours;
            activityEntity.Detail = activity.Detail;

            activityEntity.EstimatedProgress = activity.EstimatedProgress;

            activityEntity.Status = activity.Status;

            activityEntity.ModifiedBy = activity.ModifiedBy.GetValueOrDefault(0);
            activityEntity.ModifiedOn = DateTime.UtcNow;

            string oldParticipantIds = activityEntity.ParticipantIds ?? string.Empty;
            activity.ParticipantIds = activity.ParticipantIds ?? string.Empty;

            activityEntity.ParticipantIds = activity.ParticipantIds;
            activityEntity.ParticipantNames = activity.ParticipantNames;
            activityEntity.ReAssigned = activity.ReAssigned;

            //修改了参与者，则
            if (!oldParticipantIds.Equals(activity.ParticipantIds, StringComparison.OrdinalIgnoreCase))
            {
                var orgParticipantIds = new List<int>();
                var userParticipantIds = new List<int>();

                var ids = activity.ParticipantIds.Split(";".ToArray(), StringSplitOptions.RemoveEmptyEntries);
                if (ids.Length > 0)
                {
                    //var userIds = new List<int>();
                    foreach (var id in ids)
                    {
                        var pv = id.Split(new[] {'_'}, StringSplitOptions.RemoveEmptyEntries);
                        if (pv.Length != 2)
                            continue;

                        if (pv[0].ToLower() == "U".ToLower())
                        {
                            int userId;
                            if (int.TryParse(pv[1], out userId))
                            {
                                if (!userParticipantIds.Contains(userId))
                                    userParticipantIds.Add(userId);
                            }
                        }
                        else if (pv[0].ToLower() == "O".ToLower())
                        {
                            int orgId;
                            if (int.TryParse(pv[1], out orgId))
                            {
                                if (!orgParticipantIds.Contains(orgId))
                                    orgParticipantIds.Add(orgId);
                            }
                        }
                    }
                }

                var currentUserParticipants =
                    activityEntity.Participants.Where(p => p.OperateType == ActivityParticipantOperateType.User
                        /* || p.InternalOperateType == 1 不含拥有者*/).ToList();

                var addingUserParticipantIds =
                    userParticipantIds.Where(
                        id =>
                        !currentUserParticipants.Any(
                            p =>
                            (p.OperateType == ActivityParticipantOperateType.User ||
                             p.OperateType == ActivityParticipantOperateType.OwningUser) && p.ParticipantId == id));
                var removingUserParticipants =
                    currentUserParticipants.Where(x => !userParticipantIds.Contains(x.ParticipantId));

                foreach (var p in removingUserParticipants)
                {
                    activityEntity.Participants.Remove(p); //因为1:m关系，数据库方使用了Cascade Delete约束，所以无法使用Remove方法删除子对象
                    _databaseContext.ActivityParticipants.Remove(p);
                }

                foreach (var participantId in addingUserParticipantIds)
                {
                    activityEntity.Participants.Add(new ActivityParticipant
                        {
                            ActivityId = activityEntity.Id,
                            ParticipantId = participantId,
                            OperateType = ActivityParticipantOperateType.User
                        });
                }

                var currentOrgParticipants =
                    activityEntity.Participants.Where(p => p.OperateType == ActivityParticipantOperateType.Organization)
                                  .ToList();

                var addingOrgParticipantIds =
                    orgParticipantIds.Where(
                        id =>
                        !currentOrgParticipants.Any(
                            p =>
                            (p.OperateType == ActivityParticipantOperateType.User ||
                             p.OperateType == ActivityParticipantOperateType.OwningUser) && p.ParticipantId == id));
                var removingOrgParticipants =
                    currentOrgParticipants.Where(x => !orgParticipantIds.Contains(x.ParticipantId));

                foreach (var p in removingOrgParticipants)
                {
                    activityEntity.Participants.Remove(p); //因为1:m关系，数据库方使用了Cascade Delete约束，所以无法使用Remove方法删除子对象
                    _databaseContext.ActivityParticipants.Remove(p);
                }

                foreach (var participantId in addingOrgParticipantIds)
                {
                    activityEntity.Participants.Add(new ActivityParticipant
                        {
                            ActivityId = activityEntity.Id,
                            ParticipantId = participantId,
                            OperateType = ActivityParticipantOperateType.Organization
                        });
                }
            }

            try
            {
                _databaseContext.SaveChanges();
            }
            catch (Exception ex)
            {
                result.RuleViolations.Add(new RuleViolation("_FORM", string.Format("不能保存活动记录。错误信息：{0}", ex.Message)));
                return result;
            }

            result = new ServiceResult<ActivityDTO>(_activityMapper.Map(activityEntity));
            return result;
        }

        public void DeleteActivity(int id)
        {
            var activity =
                _databaseContext.Activities.Include(a => a.Reads)
                                .Include(a => a.Files)
                                .Include(a => a.Participants)
                                .FirstOrDefault(a => a.Id == id);
            if (activity == null)
                return;

            //先要删除所有子活动（回复）
            var children =
                _databaseContext.Activities.Include(a => a.Reads)
                                .Include(a => a.Files)
                                .Include(a => a.Participants)
                                .Where(a => a.ParentId == id)
                                .ToList();
            foreach (var child in children)
                _databaseContext.Activities.Remove(child);

            _databaseContext.Activities.Remove(activity);

            try
            {
                _databaseContext.SaveChanges();
            }
            catch (Exception ex)
            {
                var inner = ex;
                while (inner.InnerException != null)
                    inner = inner.InnerException;
            }
        }

        public ServiceResult<ActivityDTO> GetActivity(int id)
        {
            var result = Validation.Validate<ServiceResult<ActivityDTO>>(() => id <= 0, "id", "id参数不能小于等于0").Result();

            if (!result.RuleViolations.IsEmpty()) return result;

            var entity = _databaseContext.Activities
                                         .Include(a => a.Participants)
                                         .Include(a => a.Reads)
                                         .Include(a => a.Files)
                                         .FirstOrDefault(a => a.Id == id);

            if (entity != null && string.IsNullOrWhiteSpace(entity.ParticipantIds))
            {
                entity.ParticipantIds = string.Empty;
                entity.ParticipantNames = string.Empty;
                foreach (var activityParticipant in entity.Participants)
                {
                    if (activityParticipant.OperateType != ActivityParticipantOperateType.Organization)
                    {
                        var user =
                            _databaseContext.Users.AsNoTracking()
                                            .FirstOrDefault(a => a.Id == activityParticipant.ParticipantId);
                        if (user != null)
                        {
                            entity.ParticipantIds += "U_" + user.Id + ";";
                            entity.ParticipantNames += user.Name + ";";
                        }
                    }
                    else
                    {
                        var org =
                            _databaseContext.Organizations.AsNoTracking()
                                            .FirstOrDefault(a => a.Id == activityParticipant.ParticipantId);
                        if (org != null)
                        {
                            entity.ParticipantIds += "O_" + org.Id + ";";
                            entity.ParticipantNames += org.Name + ";";
                        }
                    }
                }
                if (entity.Participants.Count > 0)
                {
                    _databaseContext.SaveChanges();
                }
            }

            if (entity == null)
                result.RuleViolations.Add(new RuleViolation("_FORM", string.Format("不能找到指定ID{0}的活动记录", id)));
            else
                result = new ServiceResult<ActivityDTO>(_activityMapper.Map(entity));

            return result;
        }

        public Activity GetActivityEntity(int id)
        {
            return _databaseContext.Activities
                                   .FirstOrDefault(a => a.Id == id);
        }

        public IEnumerable<ActivityDTO> GetReplies(int parentId)
        {
            Check.Argument.IsNotZeroOrNegative(parentId, "parentId");
            return _databaseContext.Activities
                                   .Include(a => a.Participants)
                                   .Include(a => a.Reads)
                                   .Include(a => a.Files)
                                   .Where(a => a.ParentId == parentId)
                                   .OrderByDescending(a => a.ModifiedOn)
                                   .AsNoTracking()
                                   .ToList()
                                   .Select(a => _activityMapper.Map(a)).ToList();
        }

        public PagedResult<ActivityDTO> Find(ActivityQuery activityQuery)
        {
            Check.Argument.IsNotNull(activityQuery, "activityQuery");

            //Apply filtering
            var query = _databaseContext.Activities
                                        .Include(a => a.Participants)
                                        .Include(a => a.Reads)
                                        .Include(a => a.Files)
                                        .Where(activityQuery);

            if (activityQuery.FilterByParticipantUserId)
            {
                var orgIds =
                    _organizationsGetter.GetUserOrganizationAndAncestors(activityQuery.ParticipantUserId.Value).ToList();
                query =
                    query.Where(
                        a =>
                        a.Participants.Any(
                            p =>
                            (p.OperateType == ActivityParticipantOperateType.User ||
                             p.OperateType == ActivityParticipantOperateType.OwningUser) &&
                            p.ParticipantId == activityQuery.ParticipantUserId)
                        ||
                        a.Participants.Any(
                            p =>
                            p.OperateType == ActivityParticipantOperateType.Organization &&
                            orgIds.Contains(p.ParticipantId)));
            }

            var total = query.Count();

            //Apply sorting
            if (!string.IsNullOrWhiteSpace(activityQuery.SortMember))
            {
                if (activityQuery.SortDirection == ListSortDirection.Ascending)
                {
                    switch (activityQuery.SortMember)
                    {
                        case "Id":
                            query = query.OrderBy(u => u.Id);
                            break;
                        case "ParentId":
                            query = query.OrderBy(u => u.ParentId);
                            break;
                        case "ActivityTypeString":
                        case "ActivityType":
                            query = query.OrderBy(u => u.ActivityType);
                            break;
                        case "RegardingObjectId":
                            query = query.OrderBy(u => u.RegardingObjectId);
                            break;
                        case "RegardingObjectType":
                            query = query.OrderBy(u => u.RegardingObjectType);
                            break;
                        case "RegardingObjectUrl":
                            query = query.OrderBy(u => u.RegardingObjectUrl);
                            break;
                        case "PriorityString":
                        case "Priority":
                            query = query.OrderBy(u => u.Priority);
                            break;
                        case "QuoteOrOrderNumber":
                            query = query.OrderBy(u => u.QuoteOrOrderNumber);
                            break;
                        case "AssignedOn":
                            query = query.OrderBy(u => u.AssignedOn);
                            break;
                        case "DateDue":
                            query = query.OrderBy(u => u.DateDue);
                            break;
                        case "StartedOn":
                            query = query.OrderBy(u => u.StartedOn);
                            break;
                        case "CompletedOn":
                            query = query.OrderBy(u => u.CompletedOn);
                            break;
                        case "EstimatedHours":
                            query = query.OrderBy(u => u.EstimatedHours);
                            break;
                        case "ActualHours":
                            query = query.OrderBy(u => u.ActualHours);
                            break;
                        case "Detail":
                            query = query.OrderBy(u => u.Detail);
                            break;
                        case "Resolution":
                            query = query.OrderBy(u => u.Resolution);
                            break;
                        case "EstimatedProgress":
                            query = query.OrderBy(u => u.EstimatedProgress);
                            break;
                        case "ArticleUrl":
                            query = query.OrderBy(u => u.ArticleUrl);
                            break;
                        case "OwningUser.Name":
                        case "OwningUserId":
                            query = query.OrderBy(u => u.OwningUserId);
                            break;
                        case "OrganizationId":
                            query = query.OrderBy(u => u.OrganizationId);
                            break;
                        case "State":
                            query = query.OrderBy(u => u.State);
                            break;
                        case "StatusString":
                        case "Status":
                            query = query.OrderBy(u => u.Status);
                            break;
                        case "PlaceV4":
                            query = query.OrderBy(u => u.PlaceV4);
                            break;
                        case "CreatedByUser.Name":
                        case "CreatedBy":
                            query = query.OrderBy(u => u.CreatedBy);
                            break;
                        case "ModifiedByUser.Name":
                        case "ModifiedBy":
                            query = query.OrderBy(u => u.ModifiedBy);
                            break;
                        case "CreatedOn":
                            query = query.OrderBy(u => u.CreatedOn);
                            break;
                        case "ModifiedOn":
                            query = query.OrderBy(u => u.ModifiedOn);
                            break;
                        default:
                            query = query.OrderBy(u => u.ModifiedOn);
                            break;
                    }
                }
                else
                {
                    switch (activityQuery.SortMember)
                    {
                        case "Id":
                            query = query.OrderByDescending(u => u.Id);
                            break;
                        case "ParentId":
                            query = query.OrderByDescending(u => u.ParentId);
                            break;
                        case "ActivityTypeString":
                        case "ActivityType":
                            query = query.OrderByDescending(u => u.ActivityType);
                            break;
                        case "RegardingObjectId":
                            query = query.OrderByDescending(u => u.RegardingObjectId);
                            break;
                        case "RegardingObjectType":
                            query = query.OrderByDescending(u => u.RegardingObjectType);
                            break;
                        case "RegardingObjectUrl":
                            query = query.OrderByDescending(u => u.RegardingObjectUrl);
                            break;
                        case "PriorityString":
                        case "Priority":
                            query = query.OrderByDescending(u => u.Priority);
                            break;
                        case "QuoteOrOrderNumber":
                            query = query.OrderByDescending(u => u.QuoteOrOrderNumber);
                            break;
                        case "AssignedOn":
                            query = query.OrderByDescending(u => u.AssignedOn);
                            break;
                        case "DateDue":
                            query = query.OrderByDescending(u => u.DateDue);
                            break;
                        case "StartedOn":
                            query = query.OrderByDescending(u => u.StartedOn);
                            break;
                        case "CompletedOn":
                            query = query.OrderByDescending(u => u.CompletedOn);
                            break;
                        case "EstimatedHours":
                            query = query.OrderByDescending(u => u.EstimatedHours);
                            break;
                        case "ActualHours":
                            query = query.OrderByDescending(u => u.ActualHours);
                            break;
                        case "Detail":
                            query = query.OrderByDescending(u => u.Detail);
                            break;
                        case "Resolution":
                            query = query.OrderByDescending(u => u.Resolution);
                            break;
                        case "EstimatedProgress":
                            query = query.OrderByDescending(u => u.EstimatedProgress);
                            break;
                        case "ArticleUrl":
                            query = query.OrderByDescending(u => u.ArticleUrl);
                            break;
                        case "OwningUser.Name":
                        case "OwningUserId":
                            query = query.OrderByDescending(u => u.OwningUserId);
                            break;
                        case "OrganizationId":
                            query = query.OrderByDescending(u => u.OrganizationId);
                            break;
                        case "State":
                            query = query.OrderByDescending(u => u.State);
                            break;
                        case "StatusString":
                        case "Status":
                            query = query.OrderByDescending(u => u.Status);
                            break;
                        case "PlaceV4":
                            query = query.OrderByDescending(u => u.PlaceV4);
                            break;
                        case "CreatedByUser.Name":
                        case "CreatedBy":
                            query = query.OrderByDescending(u => u.CreatedBy);
                            break;
                        case "ModifiedByUser.Name":
                        case "ModifiedBy":
                            query = query.OrderByDescending(u => u.ModifiedBy);
                            break;
                        case "CreatedOn":
                            query = query.OrderByDescending(u => u.CreatedOn);
                            break;
                        case "ModifiedOn":
                            query = query.OrderByDescending(u => u.ModifiedOn);
                            break;
                        default:
                            query = query.OrderByDescending(u => u.ModifiedOn);
                            break;
                    }
                }
            }
            else
            {
                query = query.OrderByDescending(r => r.ModifiedOn);
            }

            //... and paging
            if (activityQuery.PageSize > 0)
            {
                query = query.Skip((activityQuery.Page - 1)*activityQuery.PageSize);
            }

            query = query.Take(activityQuery.PageSize);

            var resultSet = query.AsNoTracking().ToList().Select(r => _activityMapper.Map(r)).ToList();

            return new PagedResult<ActivityDTO>(resultSet, total);
        }

        public void AddActivityFile(int activityId, ActivityFileDTO activityFile)
        {
            var activityEntity = _databaseContext.Activities
                                                 .Include(a => a.Files)
                                                 .FirstOrDefault(a => a.Id == activityId);

            if (activityEntity == null)
                return;

            var activityFileEntity = new ActivityFile
                {
                    Content = activityFile.Content,
                    CreatedOn = activityFile.CreatedOn,
                    FileName = activityFile.FileName,
                    FileSize = activityFile.FileSize,
                    MimeType = activityFile.MimeType,
                    PathUrl = activityFile.PathUrl,
                    ActivityId = activityEntity.Id
                };
            activityEntity.Files.Add(activityFileEntity);

            try
            {
                _databaseContext.SaveChanges();
            }
            catch
            {
            }
        }

        public void DeleteActivityFile(int fileId)
        {
            var file = _databaseContext.ActivityFiles.FirstOrDefault(f => f.Id == fileId);
            if (file == null)
                return;

            //因为1:m关系，数据库方使用了Cascade Delete约束，所以无法集合导航属性activity.Files.Remove()方法删除子对象
            _databaseContext.ActivityFiles.Remove(file);
            _databaseContext.SaveChanges();
        }

        public ActivityFileDTO GetActivityFile(int fileId)
        {
            var activityFileEntity = _databaseContext.ActivityFiles.AsNoTracking().FirstOrDefault(f => f.Id == fileId);
            if (activityFileEntity == null)
                return null;

            return _activityFileMapper.Map(activityFileEntity);
        }

        public ActivityFile GetActivityFileEntity(int fileId)
        {
            return _databaseContext.ActivityFiles.FirstOrDefault(f => f.Id == fileId);
        }

        public IEnumerable<ActivityDTO> GetPastActivities(int userId)
        {
            Check.Argument.IsNotZeroOrNegative(userId, "userId");
            var now = DateTime.UtcNow.Date;
            var startDate = new DateTime(now.Year - 1, 1, 1);
            var orgIds = _organizationsGetter.GetUserOrganizationAndAncestors(userId);
            var query = _databaseContext.Activities
                                        .Include(a => a.Reads)
                                        .Where(a => a.ParentId == null && a.ModifiedOn >= startDate && a.DateDue < now);
            query = query.Where(a => !a.Reads.Any(r => r.Hidden && r.UserId == userId));
            query =
                query.Where(
                    a =>
                    a.Participants.Any(
                        p =>
                        (p.OperateType == ActivityParticipantOperateType.User ||
                         p.OperateType == ActivityParticipantOperateType.OwningUser) && p.ParticipantId == userId)
                    ||
                    a.Participants.Any(
                        p =>
                        p.OperateType == ActivityParticipantOperateType.Organization && orgIds.Contains(p.ParticipantId)));

            var result = query.OrderByDescending(a => a.ModifiedOn)
                              .Skip(0)
                              .Take(10)
                              .AsNoTracking()
                              .ToList()
                              .Select(a => _activityMapper.MapSimple(a)).ToList();

            var activityIds = result.Select(a => a.Id).ToList();
            if (activityIds.Any())
            {
                var childsCount =
                    _databaseContext.Activities.Where(a => activityIds.Contains(a.ParentId.Value))
                                    .GroupBy(a => a.ParentId)
                                    .Select(a => new {a.Key, Count = a.Count()}).ToList();

                var lastChilds =
                    _databaseContext.Activities.Where(a => activityIds.Contains(a.ParentId.Value))
                                    .GroupBy(a => a.ParentId)
                                    .Select(
                                        a =>
                                        new
                                            {
                                                a.Key,
                                                Child = a.OrderByDescending(b => b.ModifiedOn).Take(1).FirstOrDefault()
                                            })
                                    .ToList();

                foreach (var activityDTO in result)
                {
                    var childCount = childsCount.FirstOrDefault(a => a.Key == activityDTO.Id);
                    if (childCount != null)
                    {
                        activityDTO.ChildCount = childCount.Count;
                    }

                    var lastChild = lastChilds.FirstOrDefault(a => a.Key == activityDTO.Id);
                    if (lastChild != null && lastChild.Child != null)
                    {
                        var lastUserName = string.Empty;
                        if (lastChild.Child.OwningUserId > 0)
                        {
                            var lastUser = _membershipService.GetUserEntity(lastChild.Child.OwningUserId, true);
                            if (lastUser != null)
                                lastUserName = lastUser.Name;
                        }
                        activityDTO.LastMessage = string.Format("{0}({1}):{2}", lastUserName,
                                                                lastChild.Child.ModifiedOn.ToLocalTime().ToString(),
                                                                lastChild.Child.Detail);
                    }
                }
            }

            return result;
        }

        public IEnumerable<ActivityDTO> GetNowActivities(int userId)
        {
            Check.Argument.IsNotZeroOrNegative(userId, "userId");
            var now = DateTime.UtcNow.Date;

            var orgIds = _organizationsGetter.GetUserOrganizationAndAncestors(userId);

            var query = _databaseContext.Activities
                                        .Include(a => a.Reads)
                                        .Where(a => a.ParentId == null && a.DateDue == now);
            query = query.Where(a => !a.Reads.Any(r => r.Hidden && r.UserId == userId));
            query =
                query.Where(
                    a =>
                    a.Participants.Any(
                        p =>
                        (p.OperateType == ActivityParticipantOperateType.User ||
                         p.OperateType == ActivityParticipantOperateType.OwningUser) && p.ParticipantId == userId)
                    ||
                    a.Participants.Any(
                        p =>
                        p.OperateType == ActivityParticipantOperateType.Organization && orgIds.Contains(p.ParticipantId)));
            var result = query.OrderByDescending(a => a.ModifiedOn)
                              .Skip(0)
                              .Take(10)
                              .AsNoTracking()
                              .ToList()
                              .Select(a => _activityMapper.MapSimple(a)).ToList();

            var activityIds = result.Select(a => a.Id).ToList();
            if (activityIds.Any())
            {
                var childsCount =
                    _databaseContext.Activities.Where(a => activityIds.Contains(a.ParentId.Value))
                                    .GroupBy(a => a.ParentId)
                                    .Select(a => new {a.Key, Count = a.Count()}).ToList();

                var lastChilds =
                    _databaseContext.Activities.Where(a => activityIds.Contains(a.ParentId.Value))
                                    .GroupBy(a => a.ParentId)
                                    .Select(
                                        a =>
                                        new
                                            {
                                                a.Key,
                                                Child = a.OrderByDescending(b => b.ModifiedOn).Take(1).FirstOrDefault()
                                            })
                                    .ToList();

                foreach (var activityDTO in result)
                {
                    var childCount = childsCount.FirstOrDefault(a => a.Key == activityDTO.Id);
                    if (childCount != null)
                    {
                        activityDTO.ChildCount = childCount.Count;
                    }

                    var lastChild = lastChilds.FirstOrDefault(a => a.Key == activityDTO.Id);
                    if (lastChild != null && lastChild.Child != null)
                    {
                        var lastUserName = string.Empty;
                        if (lastChild.Child.OwningUserId > 0)
                        {
                            var lastUser = _membershipService.GetUserEntity(lastChild.Child.OwningUserId, true);
                            if (lastUser != null)
                                lastUserName = lastUser.Name;
                        }
                        activityDTO.LastMessage = string.Format("{0}({1}):{2}", lastUserName,
                                                                lastChild.Child.ModifiedOn.ToLocalTime().ToString(),
                                                                lastChild.Child.Detail);
                    }
                }
            }

            return result;
        }

        public IEnumerable<ActivityDTO> GetFutureActivities(int userId)
        {
            Check.Argument.IsNotZeroOrNegative(userId, "userId");
            var now = DateTime.UtcNow.Date;

            var orgIds = _organizationsGetter.GetUserOrganizationAndAncestors(userId);

            var query = _databaseContext.Activities
                                        .Include(a => a.Reads)
                                        .Where(a => a.ParentId == null && a.DateDue > now);
            query = query.Where(a => !a.Reads.Any(r => r.Hidden && r.UserId == userId));
            query =
                query.Where(
                    a =>
                    a.Participants.Any(
                        p =>
                        (p.OperateType == ActivityParticipantOperateType.User ||
                         p.OperateType == ActivityParticipantOperateType.OwningUser) && p.ParticipantId == userId)
                    ||
                    a.Participants.Any(
                        p =>
                        p.OperateType == ActivityParticipantOperateType.Organization && orgIds.Contains(p.ParticipantId)));
            var result = query.OrderByDescending(a => a.ModifiedOn)
                              .Skip(0)
                              .Take(10)
                              .AsNoTracking()
                              .ToList()
                              .Select(a => _activityMapper.MapSimple(a)).ToList();

            var activityIds = result.Select(a => a.Id).ToList();
            if (activityIds.Any())
            {
                var childsCount =
                    _databaseContext.Activities.Where(a => activityIds.Contains(a.ParentId.Value))
                                    .GroupBy(a => a.ParentId)
                                    .Select(a => new {a.Key, Count = a.Count()}).ToList();

                var lastChilds =
                    _databaseContext.Activities.Where(a => activityIds.Contains(a.ParentId.Value))
                                    .GroupBy(a => a.ParentId)
                                    .Select(
                                        a =>
                                        new
                                            {
                                                a.Key,
                                                Child = a.OrderByDescending(b => b.ModifiedOn).Take(1).FirstOrDefault()
                                            })
                                    .ToList();

                foreach (var activityDTO in result)
                {
                    var childCount = childsCount.FirstOrDefault(a => a.Key == activityDTO.Id);
                    if (childCount != null)
                    {
                        activityDTO.ChildCount = childCount.Count;
                    }

                    var lastChild = lastChilds.FirstOrDefault(a => a.Key == activityDTO.Id);
                    if (lastChild != null && lastChild.Child != null)
                    {
                        var lastUserName = string.Empty;
                        if (lastChild.Child.OwningUserId > 0)
                        {
                            var lastUser = _membershipService.GetUserEntity(lastChild.Child.OwningUserId, true);
                            if (lastUser != null)
                                lastUserName = lastUser.Name;
                        }
                        activityDTO.LastMessage = string.Format("{0}({1}):{2}", lastUserName,
                                                                lastChild.Child.ModifiedOn.ToLocalTime().ToString(),
                                                                lastChild.Child.Detail);
                    }
                }
            }

            return result;
        }

        public ServiceResult<ActivityDTO> Hidden(int activityId, int userId)
        {
            var activityEntity = _databaseContext.Activities
                                                 .Include(a => a.Reads)
                                                 .FirstOrDefault(a => a.Id == activityId);
            var result = new ServiceResult<ActivityDTO>();
            if (activityEntity == null)
                return result;

            var read = activityEntity.Reads.FirstOrDefault(p => p.UserId == userId);
            if (read != null)
            {
                read.Hidden = true; // 隐藏                
            }
            else
            {
                read = new ActivityRead
                    {
                        UserId = userId,
                        ActivityId = activityId,
                        Hidden = true
                    };
                activityEntity.Reads.Add(read);
            }

            _databaseContext.SaveChanges();
            result = new ServiceResult<ActivityDTO>(_activityMapper.Map(activityEntity));

            return result;
        }

        public ServiceResult<ActivityDTO> Read(int activityId, int userId)
        {
            var activityEntity = _databaseContext.Activities
                                                 .Include(a => a.Reads)
                                                 .FirstOrDefault(a => a.Id == activityId);
            var result = new ServiceResult<ActivityDTO>();
            if (activityEntity == null)
                return result;

            var read = activityEntity.Reads.FirstOrDefault(p => p.UserId == userId);
            if (read != null)
            {
                read.Read = true; // 隐藏                
            }
            else
            {
                read = new ActivityRead
                    {
                        UserId = userId,
                        ActivityId = activityId,
                        Read = true
                    };
                activityEntity.Reads.Add(read);
            }

            _databaseContext.SaveChanges();
            result = new ServiceResult<ActivityDTO>(_activityMapper.Map(activityEntity));

            return result;
        }
    }
}