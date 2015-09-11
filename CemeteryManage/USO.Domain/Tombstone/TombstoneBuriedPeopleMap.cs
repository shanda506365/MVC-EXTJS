using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace USO.Domain
{
    /// <summary>
    /// 墓碑客户关系表
    /// </summary>
    [Table("TombstoneBuriedPeopleMap")]
    public class TombstoneBuriedPeopleMap
    {
        [Key]
        public int Id { get; set; }
        /// <summary>
        /// 墓碑Id
        /// </summary>
        public int TombstoneId { get; set; }
        /// <summary>
        /// 埋葬人Id
        /// </summary>
        public int BuriedCustomerId { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }
    }
}
