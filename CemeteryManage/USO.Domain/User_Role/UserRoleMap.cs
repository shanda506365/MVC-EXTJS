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
    /// 用户角色关系表
    /// </summary>
    [Table("UserRoleMap")]
    public class UserRoleMap
    {
        [Key]
        public int Id { get; set; }
        /// <summary>
        /// 用户Id
        /// </summary>
        public int UserId { get; set; }
        /// <summary>
        /// 用户
        /// </summary>
        [ForeignKey("UserId")]
        public virtual User User { get; set; }
        /// <summary>
        /// 角色Id
        /// </summary>
        public int RoleId { get; set; }
        /// <summary>
        /// 角色
        /// </summary>
         [ForeignKey("RoleId")]
        public virtual Role Role { get; set; }
    }
}
