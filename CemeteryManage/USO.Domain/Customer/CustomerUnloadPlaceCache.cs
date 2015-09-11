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
    /// 联系人卸货地址缓存表
    /// </summary>
    [Table("CustomerUnloadPlaceCache")]
    public class CustomerUnloadPlaceCache
    {
        [Key]
        public int Id { get; set; }

        [StringLength(50)]
        [Required]
        public string R3CustomerCode { get; set; }

        [StringLength(50)]
        [Required]
        public string CompanyCode { get; set; }

        [StringLength(150)]
        public string Name { get; set; }

        [StringLength(250)]
        public string UnloadPlace { get; set; }

        public DateTime LastModifyTime { get; set; }
    }
}
