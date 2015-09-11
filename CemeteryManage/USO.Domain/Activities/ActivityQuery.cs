
namespace USO.Domain
{
    using System;
    using System.ComponentModel;
    using System.Linq;

    public class ActivityQuery
    {
        public ActivityQuery()
        {
            SortDirection = ListSortDirection.Ascending;
            PageSize = 50;
            Page = 1;
        }
    
        public ListSortDirection SortDirection { get; set; }
        public string SortMember { get; set; }
    
        //query.Skip((Page - 1) * PageSize);
        public int Page { get; set; }
        public int PageSize { get; set; }
        public bool IgnoreFilter { get; set; }
    
        public int? Id { get; set; }
        public bool FilterById { get { return Id.HasValue && Id.Value > 0; } }
        public ActivityTypes ActivityType { get; set; }
        public bool FilterByActivityType { get { return ActivityType != ActivityTypes.None; } }        
        public int? ParentId { get; set; }
        public bool FilterByParentId { get { return ParentId.HasValue && ParentId.Value > 0; } }        
        public int? RegardingObjectId { get; set; }
        public bool FilterByRegardingObjectId { get { return RegardingObjectId.HasValue && RegardingObjectId.Value > 0; } }        
        public short? RegardingObjectType { get; set; }
        public bool FilterByRegardingObjectType { get { return RegardingObjectType.HasValue && RegardingObjectType.Value > 0; } }
        public ActivityPriorities Priority { get; set; }
        public bool FilterByPriority { get { return Priority != ActivityPriorities.None; } }        
        public string QuoteOrOrderNumber { get; set; }
        public bool FilterByQuoteOrOrderNumber { get { return !string.IsNullOrWhiteSpace(QuoteOrOrderNumber); } }        
        public string Title { get; set; }
        public bool FilterByTitle { get { return !string.IsNullOrWhiteSpace(Title); } }

        public int? OwningUserId { get; set; }
        public bool FilterByOwningUserId { get { return OwningUserId.HasValue && OwningUserId.Value > 0; } }
        public int? OrganizationId { get; set; }
        public bool FilterByOrganizationId { get { return OrganizationId.HasValue && OrganizationId.Value > 0; } }
        public short? State { get; set; }
        public bool FilterByState { get { return State.HasValue && State.Value > 0; } }
        public ActivityStatuses Status { get; set; }
        public bool FilterByStatus { get { return Status != ActivityStatuses.None; } }
        public int? CreatedBy { get; set; }
        public bool FilterByCreatedBy { get { return CreatedBy.HasValue && CreatedBy.Value > 0; } }
        public int? ModifiedBy { get; set; }
        public bool FilterByModifiedBy { get { return ModifiedBy.HasValue && ModifiedBy.Value > 0; } }     
        public int? ParticipantId { get; set; }
        public bool FilterByParticipantId { get { return ParticipantId.HasValue && ParticipantId.Value > 0; } }
        public DateTime? BeginAssignedOn { get; set; }
        public bool FilterByBeginAssignedOn { get { return BeginAssignedOn.HasValue; } }
        public DateTime? EndAssignedOn { get; set; }
        public bool FilterByEndAssignedOn { get { return EndAssignedOn.HasValue; } }
        public DateTime? BeginDateDue { get; set; }
        public bool FilterByBeginDateDue { get { return BeginDateDue.HasValue; } }
        public DateTime? EndDateDue { get; set; }
        public bool FilterByEndDateDue { get { return EndDateDue.HasValue; } }
        public DateTime? BeginStartedOn { get; set; }
        public bool FilterByBeginStartedOn { get { return BeginStartedOn.HasValue; } }
        public DateTime? EndStartedOn { get; set; }
        public bool FilterByEndStartedOn { get { return EndStartedOn.HasValue; } }
        public DateTime? BeginCompletedOn { get; set; }
        public bool FilterByBeginCompletedOn { get { return BeginCompletedOn.HasValue; } }
        public DateTime? EndCompletedOn { get; set; }
        public bool FilterByEndCompletedOn { get { return EndCompletedOn.HasValue; } }
        public DateTime? BeginModifiedOn { get; set; }
        public bool FilterByBeginModifiedOn { get { return BeginModifiedOn.HasValue; } }
        public DateTime? EndModifiedOn { get; set; }
        public bool FilterByEndModifiedOn { get { return EndModifiedOn.HasValue; } }

        public bool? IsRead { get; set; }
        public bool FilterByIsRead { get { return IsRead.HasValue; } }

        public int? CurrentUserId { get; set; }

        public int? ParticipantUserId { get; set; }
        public bool FilterByParticipantUserId { get { return ParticipantUserId.HasValue && ParticipantUserId.Value > 0; } }
        
    }

    public static class ActivityQueryableExtension
    {
        public static IQueryable<Activity> Where(this IQueryable<Activity> query, ActivityQuery activityQuery)
        {
            bool hasFilter = false;

            if (activityQuery == null)
            {
                query = query.Where(u => false);
                return query;
            }

            if (activityQuery.FilterById)
            {
                query = query.Where(r => r.Id == activityQuery.Id);
                hasFilter = true;
            }
            if (activityQuery.FilterByActivityType)
            {
                
                query = query.Where(r => r.ActivityType==activityQuery.ActivityType);
                hasFilter = true;
            } 
   
            if (activityQuery.FilterByParentId)
            {
                query = query.Where(r => r.ParentId == activityQuery.ParentId);
                hasFilter = true;
            }
            else//否则，只获取根级任务
            {
                query = query.Where(r => r.ParentId == null);
                hasFilter = true;
            }     
            if (activityQuery.FilterByRegardingObjectId)
            {
                query = query.Where(r => r.RegardingObjectId == activityQuery.RegardingObjectId);
                hasFilter = true;
            }
       
            if (activityQuery.FilterByRegardingObjectType)
            {
                query = query.Where(r => r.RegardingObjectType == activityQuery.RegardingObjectType);
                hasFilter = true;
            }

            if (activityQuery.FilterByPriority)
            {
                query = query.Where(r => r.Priority == activityQuery.Priority);
                hasFilter = true;
            }    
            if (activityQuery.FilterByQuoteOrOrderNumber)
            {
                query = query.Where(r => r.QuoteOrOrderNumber.Contains(activityQuery.QuoteOrOrderNumber));
                hasFilter = true;
            }      
            if (activityQuery.FilterByTitle)
            {
                query = query.Where(r => r.Title.Contains(activityQuery.Title));
                hasFilter = true;
            }

            if (activityQuery.FilterByOwningUserId)
            {
                query = query.Where(r => r.OwningUserId == activityQuery.OwningUserId);
                hasFilter = true;
            }
            if (activityQuery.FilterByOrganizationId)
            {
                query = query.Where(r => r.OrganizationId == activityQuery.OrganizationId);
                hasFilter = true;
            }
            if (activityQuery.FilterByState)
            {
                query = query.Where(r => r.State == activityQuery.State);
                hasFilter = true;
            }
            if (activityQuery.FilterByStatus)
            {
                query = query.Where(r => r.Status == activityQuery.Status);
                hasFilter = true;
            }
            if (activityQuery.FilterByCreatedBy)
            {
                query = query.Where(r => r.CreatedBy == activityQuery.CreatedBy);
                hasFilter = true;
            }
            if (activityQuery.FilterByModifiedBy)
            {
                query = query.Where(r => r.ModifiedBy == activityQuery.ModifiedBy);
                hasFilter = true;
            }   
            if (activityQuery.FilterByParticipantId)
            {
                query = query.Where(r => r.Participants.Any(p => p.ParticipantId == activityQuery.ParticipantId));
                hasFilter = true;
            }

            if (activityQuery.FilterByBeginModifiedOn)
            {
                var begin = activityQuery.BeginModifiedOn.Value.Date;
                query = query.Where(r => r.ModifiedOn >= begin);
                hasFilter = true;
            }
            if (activityQuery.FilterByEndModifiedOn)
            {
                var end = activityQuery.EndModifiedOn.Value.AddDays(1).Date;
                query = query.Where(r => r.ModifiedOn < end);
                hasFilter = true;
            }

            if (activityQuery.FilterByBeginAssignedOn)
            {
                var begin = activityQuery.BeginAssignedOn.Value.Date;
                query = query.Where(r => r.AssignedOn >= begin);
                hasFilter = true;
            }
            if (activityQuery.FilterByEndAssignedOn)
            {
                var end = activityQuery.EndAssignedOn.Value.AddDays(1).Date;
                query = query.Where(r => r.AssignedOn < end);
                hasFilter = true;
            }
            if (activityQuery.FilterByBeginDateDue)
            {
                var begin = activityQuery.BeginDateDue.Value.Date;
                query = query.Where(r => r.DateDue >= begin);
                hasFilter = true;
            }
            if (activityQuery.FilterByEndDateDue)
            {
                var end = activityQuery.EndDateDue.Value.AddDays(1).Date;
                query = query.Where(r => r.DateDue < end);
                hasFilter = true;
            }
            if (activityQuery.FilterByBeginStartedOn)
            {
                var begin = activityQuery.BeginStartedOn.Value.Date;
                query = query.Where(r => r.StartedOn >= begin);
                hasFilter = true;
            }
            if (activityQuery.FilterByEndStartedOn)
            {
                var end = activityQuery.EndStartedOn.Value.AddDays(1).Date;
                query = query.Where(r => r.StartedOn < end);
                hasFilter = true;
            }
            if (activityQuery.FilterByBeginCompletedOn)
            {
                var begin = activityQuery.BeginCompletedOn.Value.Date;
                query = query.Where(r => r.CompletedOn >= begin);
                hasFilter = true;
            }
            if (activityQuery.FilterByEndCompletedOn)
            {
                var end = activityQuery.EndCompletedOn.Value.AddDays(1).Date;
                query = query.Where(r => r.CompletedOn < end);
                hasFilter = true;
            }


            if (activityQuery.FilterByIsRead && activityQuery.CurrentUserId.HasValue)
            {
                query = query.Where(r => r.Reads.Any(p => p.Read == activityQuery.IsRead && p.UserId == activityQuery.CurrentUserId));

                hasFilter = true;
            } 

            if (!hasFilter && !activityQuery.IgnoreFilter)
            {
                query = query.Where(u => false);
            }

            return query;
        }
    }
}
