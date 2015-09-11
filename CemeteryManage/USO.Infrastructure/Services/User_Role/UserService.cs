using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using USO.Domain;
using USO.Dto;
using USO.Infrastructure.Mappers;

namespace USO.Infrastructure.Services
{
    public class UserService : IUserService
    {
        private readonly IDatabaseContext _databaseContext;
        private readonly UserMapper _userMapper;

        public UserService(IDatabaseContext databaseContext, UserMapper userMapper)
        {
            _databaseContext = databaseContext;
            _userMapper = userMapper;
        }

        /// <summary>
        /// 效验用户旧密码
        /// </summary>
        /// <param name="id"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public bool CheckUserPassword(int id, string password, out string msg)
        {
            msg = string.Empty;
            try
            {
                var user = _databaseContext.Users.FirstOrDefault(a => a.Id == id);
                if (user == null)
                {
                    msg = "无效的用户";
                    return false;
                }
                else
                {
                    if (user.Password == password)
                    {
                        return true;
                    }
                    else
                    {
                        msg = "旧密码错误";
                        return false;
                    }
                }
            }
            catch (Exception ex)
            {
                msg = ex.Message;
                return false;
            }
        }

        /// <summary>
        /// 效验用户登录信息
        /// </summary>
        /// <param name="userDto"></param>
        /// <returns></returns>
        public DataControlResult<UserDTO> CheckUserLogin(UserDTO userDto)
        {
            var result = new DataControlResult<UserDTO>();
            try
            {
                var user =
                    _databaseContext.Users.FirstOrDefault(a => a.LoginName == userDto.LoginName && a.Password == userDto.Password);
                if (user != null)
                {
                    result.ResultOutDto = _userMapper.Map(user, true);
                    result.code = MyErrorCode.ResOK;
                    result.msg = string.Empty;
                    result.success = true;
                }
                else
                {
                    result.ResultOutDto = null;
                    result.code = MyErrorCode.LoginFail;
                    result.msg = string.Empty;
                    result.success = false;
                }
            }
            catch (Exception ex)
            {
                result.code = MyErrorCode.ResDBError;
                result.msg = ex.Message;
                result.success = false;
                result.ResultOutDto = null;
                return result;
            }

            return result;
        }
        /// <summary>
        /// 根据Id获取用户信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public DataControlResult<UserDTO> GetUserById(int id)
        {
            var result = new DataControlResult<UserDTO>();
            try
            {
                var user =
                    _databaseContext.Users.FirstOrDefault(a => a.Id == id);
                if (user != null)
                {
                    result.ResultOutDto = _userMapper.Map(user, true);
                    result.code = MyErrorCode.ResOK;
                    result.msg = string.Empty;
                    result.success = true;
                }
                else
                {
                    result.ResultOutDto = null;
                    result.code = MyErrorCode.LoginFail;
                    result.msg = string.Empty;
                    result.success = false;
                }
            }
            catch (Exception ex)
            {
                result.code = MyErrorCode.ResDBError;
                result.msg = ex.Message;
                result.success = false;
                result.ResultOutDto = null;
                return result;
            }

            return result;
        }
        /// <summary>
        /// 查询用户
        /// </summary>
        /// <param name="userQuery"></param>
        /// <returns></returns>
        public PagedResult<UserDTO> Find(UserQuery userQuery)
        {
            Check.Argument.IsNotNull(userQuery, "userQuery");

            //Apply filtering    
            var query = _databaseContext.Users.Where(userQuery);
            if (userQuery.filter.Department != null)
            {
                query = query.Join(_databaseContext.Departments.Where(a => a.Name == userQuery.filter.Department.Name)
                    , user => user.DepartmentId, depart => depart.Id, (user, depart) => user);
            }

            var total = query.Count();
            query = SortMemeberHelper.SortingAndPaging<User>(query, userQuery.sort, userQuery.dir
                , userQuery.page, userQuery.limit);


            var resultSet = query.AsNoTracking().ToList().Select(r => _userMapper.Map(r, true)).ToList();

            return new PagedResult<UserDTO>(resultSet, total);
        }
        /// <summary>
        /// 新建用户
        /// </summary>
        /// <param name="csDto"></param>
        /// <returns></returns>
        public DataControlResult<UserDTO> Create(UserDTO csDto)
        {
            var result = new DataControlResult<UserDTO>();
            //判断是否为重复登录名
            var repeatUser =
                  _databaseContext.Users.FirstOrDefault(a => a.LoginName == csDto.LoginName);
            if (repeatUser != null)
            {
                result.code = MyErrorCode.ResDBError;
                result.msg = "重复的登录名";
                result.success = false;
                result.ResultOutDto = null;
                return result;
            }
            try
            {
                #region 赋值
                var user = new User
                {
                    Name = csDto.Name,
                    LoginName = csDto.LoginName,
                    Code = csDto.Code,
                    Remark = csDto.Remark,
                    Password = csDto.Password,
                    Position = csDto.Position,
                    Status = csDto.Status
                };

                if (csDto.CreateDate != null) user.CreateDate = (DateTime)csDto.CreateDate;

                if (csDto.DepartmentId.HasValue && csDto.DepartmentId > 0)
                {
                    user.DepartmentId = (int)csDto.DepartmentId;
                }
                //赋予权限
                if (csDto.RoleDtos.Count > 0)
                {
                    SetUserRole(csDto);
                }

                #endregion

                _databaseContext.Users.Add(user);
                _databaseContext.SaveChanges();
                result.ResultOutDto = _userMapper.Map(user);
                result.code = MyErrorCode.ResOK;
                result.msg = string.Empty;
                result.success = true;
            }
            catch (Exception ex)
            {
                result.code = MyErrorCode.ResDBError;
                result.msg = ex.Message;
                result.success = false;
                result.ResultOutDto = null;
                return result;
            }
            return result;
        }

        /// <summary>
        /// 更新用户信息
        /// </summary>
        /// <param name="csDto"></param>
        /// <returns></returns>
        public DataControlResult<UserDTO> Update(UserDTO csDto)
        {
            var result = new DataControlResult<UserDTO>();

            try
            {
                var user = _databaseContext.Users.SingleOrDefault(n => n.Id == csDto.Id);
                if (user == null)
                {
                    result.success = false;
                    result.msg = "该用户不存在";
                    result.code = MyErrorCode.ResParamError;
                    return result;
                }
                //判断是否为重复登录名
                var repeatUser =
                      _databaseContext.Users.FirstOrDefault(a => a.LoginName == csDto.LoginName);
                if (repeatUser != null && repeatUser.Id != user.Id)
                {
                    result.code = MyErrorCode.ResDBError;
                    result.msg = "重复的登录名";
                    result.success = false;
                    result.ResultOutDto = null;
                    return result;
                }
                #region 赋值

                if (csDto.Name != null)
                {
                    user.Name = csDto.Name;
                }
                if (csDto.LoginName != null)
                {
                    user.LoginName = csDto.LoginName;
                }
                if (csDto.Code != null)
                {
                    user.Code = csDto.Code;
                }
                if (csDto.Remark != null)
                {
                    user.Remark = csDto.Remark;
                }
                if (csDto.Password != null)
                {
                    user.Password = csDto.Password;
                }
                if (csDto.Position != null)
                {
                    user.Position = csDto.Position;
                }
                if (csDto.Status != null)
                {
                    user.Status = csDto.Status;
                }
                //if (csDto.CreateDate != null) user.CreateDate = (DateTime)csDto.CreateDate;

                if (csDto.DepartmentId.HasValue && csDto.DepartmentId > 0)
                {
                    user.DepartmentId = (int)csDto.DepartmentId;
                }
                //赋予权限
                if (csDto.RoleDtos.Count > 0)
                {
                    SetUserRole(csDto);
                }

                #endregion
                _databaseContext.SaveChanges();
                result.code = MyErrorCode.ResOK;
                result.ResultOutDto = _userMapper.Map(user);
                result.msg = string.Empty;
                result.success = true;
            }
            catch (Exception ex)
            {
                result.code = MyErrorCode.ResDBError;
                result.msg = ex.Message;
                result.success = false;
                return result;
            }
            return result;
        }

        private void SetUserRole(UserDTO csDto)
        {
            var userRoleMap = _databaseContext.UserRoleMaps.FirstOrDefault(a => a.UserId == csDto.Id);
            if (userRoleMap != null) //更新
            {
                userRoleMap.RoleId = csDto.RoleDtos[0].Id;
            }
            else //添加
            {
                var newUserRoleMap = new UserRoleMap
                    {
                        UserId = csDto.Id,
                        RoleId = csDto.RoleDtos[0].Id
                    };
                _databaseContext.UserRoleMaps.Add(newUserRoleMap);
            }
        }

        /// <summary>
        /// 删除用户
        /// </summary>
        /// <param name="csDtoList"></param>
        /// <returns></returns>
        public DataControlResult<UserDTO> Delete(List<UserDTO> csDtoList)
        {
            var result = new DataControlResult<UserDTO>();
            var stopFlag = false;
            try
            {
                foreach (var userDto in csDtoList)
                {
                    var user = _databaseContext.Users.SingleOrDefault(n => n.Id == userDto.Id);
                    if (user == null)
                    {
                        stopFlag = true;
                        result.success = false;
                        result.msg = "该用户不存在";
                        result.code = MyErrorCode.ResParamError;
                        break;
                    }
                    _databaseContext.Users.Remove(user);
                }

                if (!stopFlag)
                {
                    result.code = MyErrorCode.ResOK;
                    result.msg = string.Empty;
                    result.success = true;
                    _databaseContext.SaveChanges();
                }

            }
            catch (Exception ex)
            {
                result.code = MyErrorCode.ResDBError;
                result.msg = ex.Message;
                result.success = false;
                return result;
            }
            return result;
        }

    }
}
