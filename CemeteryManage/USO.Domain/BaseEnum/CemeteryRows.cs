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
    /// 墓碑行表
    /// </summary>
    [Table("CemeteryRows")]
    public class CemeteryRows
    {
         [Key]
        public int Id { get; set; }
        /// <summary>
        /// 名称
        /// </summary>
        [StringLength(200)]
        public string Name { get; set; }
        /// <summary>
        /// 别名
        /// </summary>
        [StringLength(200)]
        public string Alias { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }
    }

}
