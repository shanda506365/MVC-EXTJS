namespace USO.Domain
{
    using System.ComponentModel.DataAnnotations;

    public class UserGridSetting
    {
        [Key]
        public long Id { get; set; }

        public long OrganizationId { get; set; }

        public string GridName { get; set; }

        public string Settings { get; set; }

        public long UserId { get; set; }
    }

    public class GridSetting
    {
        public string ColumnName { get; set; }

        public int Width { get; set; }

        public bool Hide { get; set; }

        public int Order { get; set; }
    }
}