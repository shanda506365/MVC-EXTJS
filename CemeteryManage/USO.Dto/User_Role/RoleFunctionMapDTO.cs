using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using USO.Domain;

namespace USO.Dto
{
    public class RoleFunctionMapDTO
    {
        public int Id { get; set; }
        /// <summary>
        /// 角色Id
        /// </summary>
        public int RoleId { get; set; }
        /// <summary>
        /// 角色
        /// </summary>
        public  Role Role { get; set; }

        /// <summary>
        /// 功能Id
        /// </summary>
        public int FunctionId { get; set; }
        /// <summary>
        /// 功能
        /// </summary>
        public  Function Functions { get; set; }
    }
}
