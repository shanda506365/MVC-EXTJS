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
    public interface IUserService : IDependency
    {
        /// <summary>
        /// 效验用户旧密码
        /// </summary>
        /// <param name="id"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        bool CheckUserPassword(int id,string password,out string msg);

        /// <summary>
        /// 效验用户登录信息
        /// </summary>
        /// <param name="userDto"></param>
        /// <returns></returns>
        DataControlResult<UserDTO> CheckUserLogin(UserDTO userDto);

        /// <summary>
        /// 根据Id获取用户信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        DataControlResult<UserDTO> GetUserById(int id);
        /// <summary>
        /// 查询用户
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        PagedResult<UserDTO> Find(UserQuery query);

        /// <summary>
        /// 新建用户
        /// </summary>
        /// <param name="userDto"></param>
        /// <returns></returns>
        DataControlResult<UserDTO> Create(UserDTO userDto);
        /// <summary>
        /// 更新用户信息
        /// </summary>
        /// <param name="userDto"></param>
        /// <returns></returns>
        DataControlResult<UserDTO> Update(UserDTO userDto);
        /// <summary>
        /// 删除用户
        /// </summary>
        /// <param name="userDtoList"></param>
        /// <returns></returns>
        DataControlResult<UserDTO> Delete(List<UserDTO> userDtoList);

    }
}
