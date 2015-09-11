using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace USO.Dto
{
    public class RoleDTO
    {
        public int Id { get; set; }
        /// <summary>
        /// 角色名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 该角色包含的功能
        /// </summary>
        private List<FunctionDTO> _functionDtos;
        /// <summary>
        /// 该角色包含的功能
        /// </summary>
        public List<FunctionDTO> FunctionDtos
        {
            get
            {
                if (this._functionDtos != null)
                {
                    return this._functionDtos;
                }
                else
                {
                    return null;
                }

            }
            set { this._functionDtos = value; }
        }

        /// <summary>
        /// 该角色包含的功能文本
        /// </summary>
        public string FunctionsString
        {
            get
            {
                var functionName = from p in FunctionDtos select p.Name;
                string str = string.Join(",", functionName.ToArray());
                return str;
            }
        }
    }
}
