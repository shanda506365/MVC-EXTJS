
namespace USO.Dto.Activities
{
    using System;
    using System.Collections.Generic;
    using USO.Domain;

    public class ActivityDTO
    {
        public ActivityDTO()
        {
            this.Participants = new List<ActivityParticipantDTO>();
            this.Files = new List<ActivityFileDTO>();
        }
      
        public long Id { get; set; }
        public ActivityTypes? ActivityType { get; set; }
        public string ActivityTypeString
        {
            get
            {
                return ActivityType.GetValueOrDefault(ActivityTypes.None).Display();
            }
        }
        public long? ParentId { get; set; }
        public long? RegardingObjectId { get; set; }
        public short? RegardingObjectType { get; set; }
        public string RegardingObjectUrl { get; set; }
        public ActivityPriorities? Priority { get; set; }
        public string PriorityString
        {
            get
            {
                return Priority.GetValueOrDefault(ActivityPriorities.None).Display();
            }
        }
        public string QuoteOrOrderNumber { get; set; }
        public string Title { get; set; }
        public DateTime? AssignedOn { get; set; }
        public DateTime? DateDue { get; set; }
        public DateTime? StartedOn { get; set; }
        public DateTime? CompletedOn { get; set; }
        public int? EstimatedHours { get; set; }
        public int? ActualHours { get; set; }
        public string Detail { get; set; }
        public string Resolution { get; set; }
        public int? EstimatedProgress { get; set; }
        public string ArticleUrl { get; set; }
        public long OwningUserId { get; set; }
        public string OwningUserName { get; set; }

        public long? OrganizationId { get; set; }
        public short? State { get; set; }
        public ActivityStatuses? Status { get; set; }
        public string StatusString
        {
            get
            {
                return Status.GetValueOrDefault(ActivityStatuses.None).Display();
            }
        }
        public string PlaceV4 { get; set; }
        public long CreatedBy { get; set; }
        public string CreatedByUserName{ get; set; }

        public long ModifiedBy { get; set; }
        public string ModifiedByUserName { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime ModifiedAt { get; set; }
        public bool? ReAssigned { get; set; }
        public int ChildCount { get; set; }

        public string ParticipantIds { get; set; }
        public string ParticipantNames { get; set; }

        public IList<ActivityParticipantDTO> Participants { get; set; }
        public IList<ActivityFileDTO> Files { get; set; }

        public bool IsRead { get; set; }

        public string LastMessage { get; set; }
    }
}

