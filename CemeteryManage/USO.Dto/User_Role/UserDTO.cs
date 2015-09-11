using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using USO.Domain;

namespace USO.Dto
{
    /// <summary>
    /// 用户表
    /// </summary>
    public class UserDTO
    {
        public int Id { get; set; }
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 部门Id
        /// </summary>
        public int? DepartmentId { get; set; }
       
        public DepartmentDTO DepartmentEntity { get; set; }
        /// <summary>
        /// 登录名
        /// </summary>
        public string LoginName { get; set; }
        /// <summary>
        /// 编号
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// 职务
        /// </summary>
        public string Position { get; set; }

        /// <summary>
        /// 创建日期
        /// </summary>
        public DateTime? CreateDate { get; set; }
        public string CreateDateString
        {
            get
            {
                return CreateDate.HasValue ? ((DateTime)CreateDate).ToString("yyyy-MM-dd HH:mm:ss") : "";
            }
        }

        /// <summary>
        /// 状态
        /// </summary>
        public UserStatus? Status { get; set; }

        public string StatusString
        {
            get { return Status.DescriptionFor(); }
        }

        /// <summary>
        /// 该用户包含的角色
        /// </summary>
        public List<RoleDTO> RoleDtos { get; set; }

        /// <summary>
        /// 该用户包含的角色文本
        /// </summary>
        public string RoleDtosString
        {
            get
            {
                string str = string.Empty;
                if (RoleDtos != null)
                {
                    var roles = from p in RoleDtos select p.Name;
                    str = string.Join(",", roles.ToArray());
                }
                return str;
            }
        }
    }
}
