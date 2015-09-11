using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace USO.Domain.Customer
{
    /// <summary>
    /// 客户联系人表缓存表
    /// </summary>
    [Table("CustomerContactCache")]
    public class CustomerContactCache
    {
        [Key]
        public int Id { get; set; }

        [StringLength(50)]
        [Required]
        public string R3CustomerCode { get; set; }

        [StringLength(50)]
        [Required]
        public string CompanyCode { get; set; }

        [StringLength(50)]
        public string ContactName { get; set; }

        [StringLength(50)]
        public string ContactPhone { get; set; }

        public DateTime LastModifyTime { get; set; }
    }
}
