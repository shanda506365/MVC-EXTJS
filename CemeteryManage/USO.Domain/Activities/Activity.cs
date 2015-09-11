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
        //工作安排(默认）
        Schedule = 1,
        //订单问题
        OrderIssue = 2,
        //私人
        Personal = 3,
        //公开
        Open = 4,
        //研讨
        Discussion = 5,
        //其他
        Other = 6,
    }

    public static class ActivityTypesExtensions
    {
        public static string Display(this ActivityTypes type)
        {
            switch (type)
            {
                case ActivityTypes.None:
                    return "无";
                case ActivityTypes.Schedule:
                    return "工作安排";
                case ActivityTypes.OrderIssue:
                    return "订单问题";
                case ActivityTypes.Personal:
                    return "私人";
                case ActivityTypes.Open:
                    return "公开";
                case ActivityTypes.Discussion:
                    return "研讨";
                case ActivityTypes.Other:
                    return "其它";
                default:
                    return "未知";
            }
        }
    }

    public enum ActivityStatuses
    {
        None = 0,
        //进行中
        Processing = 1,
        //未开始(默认）
        Init = 2,
        //等待中
        Waiting = 3,
        //取消
        Canceled = 4,
        //完成
        Completed = 5,
    }

    public static class ActivityStatusesExtensions
    {
        public static string Display(this ActivityStatuses status)
        {
            switch (status)
            {
                case ActivityStatuses.None:
                    return "无";
                case ActivityStatuses.Processing:
                    return "进行中";
                case ActivityStatuses.Init:
                    return "未开始";
                case ActivityStatuses.Waiting:
                    return "等待中";
                case ActivityStatuses.Canceled:
                    return "取消";
                case ActivityStatuses.Completed:
                    return "完成";
                default:
                    return "未知";
            }
        }
    }

    //不是自己拥有的任务，有直接/间接区分
    public enum ActivityDirects
    {
        None = 0,
        //直接(默认）
        Direct = 1,
        //间接
        Indirect = 2,
    }

    public static class ActivityDirectsExtensions
    {
        public static string Display(this ActivityDirects direct)
        {
            switch (direct)
            {
                case ActivityDirects.None:
                    return "无";
                case ActivityDirects.Direct:
                    return "进直接";
                case ActivityDirects.Indirect:
                    return "间接";
                default:
                    return "未知";
            }
        }
    }

    public enum ActivityPriorities
    {
        None = 0,
        //紧急
        Emergent = 1,
        //高
        High = 2,
        //中
        Medium = 3,
        //低(默认）
        Low = 4,
    }

    public static class ActivityPrioritiesExtensions
    {
        public static string Display(this ActivityPriorities priority)
        {
            switch (priority)
            {
                case ActivityPriorities.None:
                    return "无";
                case ActivityPriorities.Emergent:
                    return "紧急";
                case ActivityPriorities.High:
                    return "高";
                case ActivityPriorities.Medium:
                    return "中";
                case ActivityPriorities.Low:
                    return "低";
                default:
                    return "未知";
            }
        }
    }

    public enum ActivityRanges
    {
        None = 0,
        //我发出的任务(默认)
        Owning = 1,
        //我收到的任务
        Assigned = 2,
    }

    public static class ActivityRangesExtensions
    {
        public static string Display(this ActivityRanges range)
        {
            switch (range)
            {
                case ActivityRanges.None:
                    return "无";
                case ActivityRanges.Owning:
                    return "我发出的任务";
                case ActivityRanges.Assigned:
                    return "我收到的任务";
                default:
                    return "未知";
            }
        }
    }
}