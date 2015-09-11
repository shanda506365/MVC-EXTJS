namespace USO.Dto.Activities
{
    using USO.Domain;

    public class ActivityParticipantDTO
    {
        public long Id { get; set; }

        public long ActivityId { get; set; }

        public ActivityParticipantOperateType OperateType { get; set; }

        public long ParticipantId { get; set; }

        public string ParticipantName { get; set; }
    }
}

