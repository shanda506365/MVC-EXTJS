namespace USO.Domain
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;


    public class ActivityParticipant
    {
        [Key]
        public int Id { get; set; }

        public int ActivityId { get; set; }
        public int ParticipantId { get; set; }
        
        public ActivityParticipantOperateType OperateType { get; set; }
    }

    public enum ActivityParticipantOperateType
    {
        User = 0,
        OwningUser = 1,
        Organization = 2
    }
}