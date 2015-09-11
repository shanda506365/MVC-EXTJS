using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using USO.Core;
using USO.Domain;
using USO.Dto;

namespace USO.Infrastructure.Services
{
    public interface IRoleService : IDependency
    {
        /// <summary>
        /// 查询角色
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        PagedResult<RoleDTO> Find(RoleQuery query);

        /// <summary>
        /// 新建角色
        /// </summary>
        /// <param name="roleDto"></param>
        /// <returns></returns>
        DataControlResult<RoleDTO> Create(RoleDTO roleDto);
        /// <summary>
        /// 更新角色信息
        /// </summary>
        /// <param name="roleDto"></param>
        /// <returns></returns>
        DataControlResult<RoleDTO> Update(RoleDTO roleDto);
        /// <summary>
        /// 删除角色
        /// </summary>
        /// <param name="roleDtoList"></param>
        /// <returns></returns>
        DataControlResult<RoleDTO> Delete(List<RoleDTO> roleDtoList);
    }
}
