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
    /// 墓碑列 表
    /// </summary>
    [Table("CemeteryColumns")]
    public class CemeteryColumns
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
