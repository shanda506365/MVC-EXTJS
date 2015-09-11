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
    /// 角色功能关系表
    /// </summary>
    [Table("RoleFunctionMap")]
    public class RoleFunctionMap
    {
        [Key]
        public int Id { get; set; }
        /// <summary>
        /// 角色Id
        /// </summary>
        public int RoleId { get; set; }
        /// <summary>
        /// 角色
        /// </summary>
        [ForeignKey("RoleId")]
        public virtual Role Role { get; set; }

        /// <summary>
        /// 功能Id
        /// </summary>
        public int FunctionId { get; set; }
        /// <summary>
        /// 功能
        /// </summary>
        [ForeignKey("FunctionId")]
        public virtual Function Functions { get; set; }
    }
}
