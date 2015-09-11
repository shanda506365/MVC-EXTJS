
namespace USO.Dto.Activities
{
    using System;

    public class ActivityFileDTO
    {
        public long Id { get; set; }

        public long ActivityId { get; set; }

        public byte[] Content { get; set; }

        public DateTime CreatedOn { get; set; }

        public string FileName { get; set; }

        public int? FileSize { get; set; }

        public string MimeType { get; set; }

        public string PathUrl { get; set; }

        public string FileSizeString
        {
            get
            {
                var size = FileSize.HasValue ? FileSize.Value : 0;
                var str = string.Empty;

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

