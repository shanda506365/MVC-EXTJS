namespace USO.Infrastructure.Mappers
{
    using System.Data.Entity;
    using System.Linq;
    using USO.Core;
    using USO.Domain;
    using USO.Dto.Activities;
    using USO.Infrastructure.Security;

    public interface IActivityMapper : IDependency 
    {
        ActivityDTO Map(Activity entity);

        ActivityDTO MapSimple(Activity entity);
    }
    
    public class ActivityMapper : IActivityMapper
    {
        private readonly WorkContext _workContext;
        private readonly IDatabaseContext _databaseContext;
        private readonly IActivityParticipantMapper _activityParticipantMapper;
        private readonly IActivityFileMapper _activityFileMapper;
        private readonly IMembershipService _membershipService;

        public ActivityMapper(
            WorkContext workContext,
            IDatabaseContext databaseContext,
            IActivityParticipantMapper activityParticipantMapper,
            IActivityFileMapper activityFileMapper,
            IMembershipService membershipService
            )
        {
            _workContext = workContext;
            _databaseContext = databaseContext;
            _activityParticipantMapper = activityParticipantMapper;
            _activityFileMapper = activityFileMapper;
            _membershipService = membershipService;
        }

        public ActivityDTO Map(Activity entity)
        {
            var dto = LoadEntityData(entity);

            if (entity.Participants != null && entity.Participants.Count > 0)
                dto.Participants = entity.Participants.Select(p => _activityParticipantMapper.Map(p)).ToList();
            if (entity.Files != null && entity.Files.Count>0)
                dto.Files = entity.Files.OrderByDescending(f => f.CreatedOn).Select(f => _activityFileMapper.Map(f)).ToList();

            dto.ChildCount = _databaseContext.Activities.Where(a => a.ParentId == entity.Id).Count();

            if (dto.ChildCount > 0)
            {
                var lastChild = _databaseContext.Activities.AsNoTracking().Where(a => a.ParentId == entity.Id).OrderByDescending(a => a.ModifiedOn).Take(1).FirstOrDefault();
                if (lastChild != null)
                {
                    var lastUserName = string.Empty;
                    if (lastChild.OwningUserId > 0)
                    {
                        var lastUser = _membershipService.GetUserEntity(lastChild.OwningUserId, true);
                        if (lastUser != null)
                            lastUserName = lastUser.Name;
                    }
                    dto.LastMessage=string.Format("{0}({1}):{2}", lastUserName, lastChild.ModifiedOn.ToLocalTime().ToString(), lastChild.Detail);
                }
            }

            if (entity.Reads != null && entity.Reads.Count > 0)
                dto.IsRead = entity.Reads.Any(p => p.UserId == _workContext.CurrentUser.Id && p.Read == true);

            if (entity.OwningUserId > 0)
            {
                var user = _membershipService.GetUserEntity(entity.OwningUserId, true);
                if (user != null)
                    dto.OwningUserName = user.Name;
            }
            return dto;
    	 }

        public ActivityDTO MapSimple(Activity entity)
        {
            var dto = LoadEntityData(entity);

            if (entity.Reads != null && entity.Reads.Count > 0)
                dto.IsRead = entity.Reads.Any(p => p.UserId == _workContext.CurrentUser.Id && p.Read);

            if (entity.OwningUserId > 0)
            {
                var user = _membershipService.GetUserEntity(entity.OwningUserId, true);
                if (user != null)
                    dto.OwningUserName = user.Name;
            }
            return dto;
        }

        private ActivityDTO LoadEntityData(Activity entity)
        {
            return new ActivityDTO
                          {
                                Id = entity.Id,
                                ActivityType = entity.ActivityType,
                                ParentId = entity.ParentId,
                                RegardingObjectId = entity.RegardingObjectId,
                                RegardingObjectType = entity.RegardingObjectType,
                                RegardingObjectUrl = entity.RegardingObjectUrl,
                                Priority = entity.Priority,
                                QuoteOrOrderNumber = entity.QuoteOrOrderNumber,
                                Title = entity.Title,
                                AssignedOn = entity.AssignedOn,
                                DateDue = entity.DateDue,
                                StartedOn = entity.StartedOn,
                                CompletedOn = entity.CompletedOn,
                                EstimatedHours = entity.EstimatedHours,
                                ActualHours = entity.ActualHours,
                                Detail = entity.Detail,
                                Resolution = entity.Resolution,
                                EstimatedProgress = entity.EstimatedProgress,
                                ArticleUrl = entity.ArticleUrl,
                                OwningUserId = entity.OwningUserId,
                                OrganizationId = entity.OrganizationId,
                                State = entity.State,
                                Status = entity.Status,
                                PlaceV4 = entity.PlaceV4,
                                CreatedBy = entity.CreatedBy,
                                CreatedAt = entity.CreatedOn,
                                ModifiedBy = entity.ModifiedBy,
                                ModifiedAt = entity.ModifiedOn,    
                                ReAssigned = entity.ReAssigned,
                                ParticipantIds = entity.ParticipantIds,
                                ParticipantNames = entity.ParticipantNames
                          };
        }
    }
}
