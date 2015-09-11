namespace USO.Domain
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class ActivityRead
    {
        public ActivityRead()
        {
            CreatedOn = DateTime.UtcNow;
        }


        [Key]
        public int Id { get; set; }

        public int ActivityId { get; set; }
        public int UserId { get; set; }
        public bool Hidden { get; set; }
        public bool Read { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}