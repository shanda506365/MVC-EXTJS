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
    public class RoleService : IRoleService
    {
        private readonly IDatabaseContext _databaseContext;
        private readonly RoleMapper _roleMapper;

        public RoleService(IDatabaseContext databaseContext, RoleMapper roleMapper)
        {
            _databaseContext = databaseContext;
            _roleMapper = roleMapper;
        }

        /// <summary>
        /// 查询角色
        /// </summary>
        /// <param name="roleQuery"></param>
        /// <returns></returns>
        public PagedResult<RoleDTO> Find(RoleQuery roleQuery)
        {
            Check.Argument.IsNotNull(roleQuery, "roleQuery");

            //Apply filtering    
            var query = _databaseContext.Roles.Where(roleQuery);

            var total = query.Count();
            query = SortMemeberHelper.SortingAndPaging<Role>(query, roleQuery.sort, roleQuery.dir
                , roleQuery.page, roleQuery.limit);


            var resultSet = query.AsNoTracking().ToList().Select(r => _roleMapper.Map(r)).ToList();

            return new PagedResult<RoleDTO>(resultSet, total);
        }
        /// <summary>
        /// 新建角色
        /// </summary>
        /// <param name="csDto"></param>
        /// <returns></returns>
        public DataControlResult<RoleDTO> Create(RoleDTO csDto)
        {
            var result = new DataControlResult<RoleDTO>();
            //判断是否为重复登录名
            var repeatRole =
                  _databaseContext.Roles.FirstOrDefault(a => a.Name == csDto.Name);
            if (repeatRole != null)
            {
                result.code = MyErrorCode.ResDBError;
                result.msg = "重复的角色名";
                result.success = false;
                result.ResultOutDto = null;
                return result;
            }
            using (TransactionScope tsScope = new TransactionScope())
            {
                try
                {
                    #region 赋值
                    var role = new Role
                    {
                        Name = csDto.Name
                    };
                    if (csDto.FunctionDtos != null)
                    {
                        SetRolePermissionsFunctionMap(csDto);
                    }
                    #endregion

                    _databaseContext.Roles.Add(role);
                    _databaseContext.SaveChanges();
                    result.ResultOutDto = _roleMapper.Map(role);
                    result.code = MyErrorCode.ResOK;
                    result.msg = string.Empty;
                    result.success = true;
                    tsScope.Complete();
                }
                catch (Exception ex)
                {
                    result.code = MyErrorCode.ResDBError;
                    result.msg = ex.Message;
                    result.success = false;
                    result.ResultOutDto = null;
                    return result;
                }
            }
            return result;
        }

        /// <summary>
        /// 更新角色信息
        /// </summary>
        /// <param name="csDto"></param>
        /// <returns></returns>
        public DataControlResult<RoleDTO> Update(RoleDTO csDto)
        {
            var result = new DataControlResult<RoleDTO>();
            using (TransactionScope tsScope = new TransactionScope())
            {
                try
                {
                    var role = _databaseContext.Roles.SingleOrDefault(n => n.Id == csDto.Id);
                    if (role == null)
                    {
                        result.success = false;
                        result.msg = "该角色不存在";
                        result.code = MyErrorCode.ResParamError;
                        return result;
                    }

                    #region 赋值

                    if (csDto.Name != null)
                    {
                        role.Name = csDto.Name;
                    }

                    #endregion

                    _databaseContext.SaveChanges();
                    result.code = MyErrorCode.ResOK;
                    result.ResultOutDto = _roleMapper.Map(role);
                    result.msg = string.Empty;
                    result.success = true;
                    //赋予权限
                    if (csDto.FunctionDtos != null)
                    {
                        csDto.Id = result.ResultOutDto.Id;
                        SetRolePermissionsFunctionMap(csDto);
                        _databaseContext.SaveChanges();
                    }
                    tsScope.Complete();

                }
                catch (Exception ex)
                {
                    result.code = MyErrorCode.ResDBError;
                    result.msg = ex.Message;
                    result.success = false;
                    return result;
                }
            }
            return result;
        }

        private void SetRolePermissionsFunctionMap(RoleDTO csDto)
        {
            var roleFunctionMaps = _databaseContext.RoleFunctionMaps.Where(a => a.RoleId == csDto.Id);

            foreach (var roleFunctionMap in roleFunctionMaps)
            {
                _databaseContext.RoleFunctionMaps.Remove(roleFunctionMap);
            }


            foreach (var functionDto in csDto.FunctionDtos)
            {
                var roleFunctionMap = new RoleFunctionMap()
                    {
                        FunctionId = functionDto.Id,
                        RoleId = csDto.Id
                    };
                _databaseContext.RoleFunctionMaps.Add(roleFunctionMap);
            }
        }

        /// <summary>
        /// 删除角色
        /// </summary>
        /// <param name="csDtoList"></param>
        /// <returns></returns>
        public DataControlResult<RoleDTO> Delete(List<RoleDTO> csDtoList)
        {
            var result = new DataControlResult<RoleDTO>();
            var stopFlag = false;
            try
            {
                foreach (var roleDto in csDtoList)
                {
                    var role = _databaseContext.Roles.SingleOrDefault(n => n.Id == roleDto.Id);
                    if (role == null)
                    {
                        stopFlag = true;
                        result.success = false;
                        result.msg = "该角色不存在";
                        result.code = MyErrorCode.ResParamError;
                        break;
                    }
                    _databaseContext.Roles.Remove(role);
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
