namespace USO.Domain
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class Activity
    {
        public Activity()
        {
            this.CreatedOn = DateTime.UtcNow;
            this.ModifiedOn = DateTime.UtcNow;
            this.Files = new List<ActivityFile>();
            this.Participants = new List<ActivityParticipant>();
            this.Reads = new List<ActivityRead>();
        }


        [Key]
        public int Id { get; set; }


        public ActivityTypes? ActivityType { get; set; }

        public int? ParentId { get; set; }
        public int? RegardingObjectId { get; set; }
        public short? RegardingObjectType { get; set; }

        [StringLength(255)]
        public string RegardingObjectUrl { get; set; }


        public ActivityPriorities? Priority { get; set; }

        [StringLength(100)]
        public string QuoteOrOrderNumber { get; set; }

        [StringLength(255)]
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

        [StringLength(255)]
        public string ArticleUrl { get; set; }

        public int OwningUserId { get; set; }
        public int? OrganizationId { get; set; }
        public short? State { get; set; }


        public ActivityStatuses? Status { get; set; }

        public string PlaceV4 { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public int ModifiedBy { get; set; }
        public DateTime ModifiedOn { get; set; }
        public byte[] VersionNumber { get; set; }

        [StringLength(1000)]
        public string ParticipantIds { get; set; }

        [StringLength(int.MaxValue)]
        public string ParticipantNames { get; set; }

        public bool? ReAssigned { get; set; }


        public IList<ActivityFile> Files { get; set; }
        public IList<ActivityParticipant> Participants { get; set; }
        public IList<ActivityRead> Reads { get; set; }
    }

    public enum ActivityTypes
    {
        None = 0,
        //��������(Ĭ�ϣ�
        Schedule = 1,
        //��������
        OrderIssue = 2,
        //˽��
        Personal = 3,
        //����
        Open = 4,
        //����
        Discussion = 5,
        //����
        Other = 6,
    }

    public static class ActivityTypesExtensions
    {
        public static string Display(this ActivityTypes type)
        {
            switch (type)
            {
                case ActivityTypes.None:
                    return "��";
                case ActivityTypes.Schedule:
                    return "��������";
                case ActivityTypes.OrderIssue:
                    return "��������";
                case ActivityTypes.Personal:
                    return "˽��";
                case ActivityTypes.Open:
                    return "����";
                case ActivityTypes.Discussion:
                    return "����";
                case ActivityTypes.Other:
                    return "����";
                default:
                    return "δ֪";
            }
        }
    }

    public enum ActivityStatuses
    {
        None = 0,
        //������
        Processing = 1,
        //δ��ʼ(Ĭ�ϣ�
        Init = 2,
        //�ȴ���
        Waiting = 3,
        //ȡ��
        Canceled = 4,
        //���
        Completed = 5,
    }

    public static class ActivityStatusesExtensions
    {
        public static string Display(this ActivityStatuses status)
        {
            switch (status)
            {
                case ActivityStatuses.None:
                    return "��";
                case ActivityStatuses.Processing:
                    return "������";
                case ActivityStatuses.Init:
                    return "δ��ʼ";
                case ActivityStatuses.Waiting:
                    return "�ȴ���";
                case ActivityStatuses.Canceled:
                    return "ȡ��";
                case ActivityStatuses.Completed:
                    return "���";
                default:
                    return "δ֪";
            }
        }
    }

    //�����Լ�ӵ�е�������ֱ��/�������
    public enum ActivityDirects
    {
        None = 0,
        //ֱ��(Ĭ�ϣ�
        Direct = 1,
        //���
        Indirect = 2,
    }

    public static class ActivityDirectsExtensions
    {
        public static string Display(this ActivityDirects direct)
        {
            switch (direct)
            {
                case ActivityDirects.None:
                    return "��";
                case ActivityDirects.Direct:
                    return "��ֱ��";
                case ActivityDirects.Indirect:
                    return "���";
                default:
                    return "δ֪";
            }
        }
    }

    public enum ActivityPriorities
    {
        None = 0,
        //����
        Emergent = 1,
        //��
        High = 2,
        //��
        Medium = 3,
        //��(Ĭ�ϣ�
        Low = 4,
    }

    public static class ActivityPrioritiesExtensions
    {
        public static string Display(this ActivityPriorities priority)
        {
            switch (priority)
            {
                case ActivityPriorities.None:
                    return "��";
                case ActivityPriorities.Emergent:
                    return "����";
                case ActivityPriorities.High:
                    return "��";
                case ActivityPriorities.Medium:
                    return "��";
                case ActivityPriorities.Low:
                    return "��";
                default:
                    return "δ֪";
            }
        }
    }

    public enum ActivityRanges
    {
        None = 0,
        //�ҷ���������(Ĭ��)
        Owning = 1,
        //���յ�������
        Assigned = 2,
    }

    public static class ActivityRangesExtensions
    {
        public static string Display(this ActivityRanges range)
        {
            switch (range)
            {
                case ActivityRanges.None:
                    return "��";
                case ActivityRanges.Owning:
                    return "�ҷ���������";
                case ActivityRanges.Assigned:
                    return "���յ�������";
                default:
                    return "δ֪";
            }
        }
    }
}