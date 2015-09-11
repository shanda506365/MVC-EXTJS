﻿
namespace USO.Dto.Activities
{
    using System;
    using USO.Domain;

    
    public class ActivityCreateParams
    {
        //任务类型
        public ActivityTypes? ActivityType { get; set; }
        //如果是答复，则
        public int? ParentId { get; set; }
        //优先级
        public ActivityPriorities? Priority { get; set; }
        //任务标题
        public string Title { get; set; }
        //开始日期
        public DateTime? AssignedOn { get; set; }
        //预计结束日期
        public DateTime? DateDue { get; set; }
        //开始日期
        public DateTime? StartedOn { get; set; }
        //实际完成日期
        public DateTime? CompletedOn { get; set; }
        //实际耗时
        public int? ActualHours { get; set; }
        //任务详细信息
        public string Detail { get; set; }
        //进度
        public int? EstimatedProgress { get; set; }
        //拥有者
        public int OwningUserId { get; set; }
        public string OwningUserName { get; set; }
        public int? OrganizationId { get; set; }
        //状态
        public ActivityStatuses? Status { get; set; }
        //是否能再分派
        public bool? ReAssigned { get; set; }

        //参与者Ids(分号分隔): Id1_U;Id2_O
        public string ParticipantIds { get; set; }

        //参与者名字(分号分隔):Nmae1;Name2
        public string ParticipantNames { get; set; }
    }
}
