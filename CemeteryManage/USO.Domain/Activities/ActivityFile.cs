
namespace USO.Domain
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
   
    public class ActivityFile
    {
        public ActivityFile()
        {
            this.CreatedOn = DateTime.UtcNow;
        }
       
        [Key]
        public int Id { get; set; }
        public int ActivityId { get; set; }
        public string PathUrl { get; set; }
        public int? FileSize { get; set; }
        public string FileName { get; set; }
        public string MimeType { get; set; }
        public byte[] Content { get; set; }
        public DateTime CreatedOn { get; set; }

        [NotMapped]
        public string FileSizeString
        {
            get
            {
                int size = FileSize.HasValue ? FileSize.Value : 0;
                string str = "";

                if (size < 1000)
                {
                    str = size.ToString() + "Byte(s)";
                }
                else
                {
                    size = size / 1024;
                    str = size.ToString() + "KB";
                }
                return "(" + str + ")";
            }
        }
    }
}
