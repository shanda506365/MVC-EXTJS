using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using USO.Core;
using USO.Domain;
using USO.Dto;

namespace USO.Infrastructure.Mappers
{
    public interface IUserMapper : IDependency
    {
        UserDTO Map(User entity);
    }

    public class UserMapper : IUserMapper
    {
        private readonly IDatabaseContext _databaseContext;
        private readonly RoleMapper _roleMapper;

        public UserMapper(IDatabaseContext databaseContext,RoleMapper roleMapper)
         {
             _databaseContext = databaseContext;
             _roleMapper = roleMapper;
         }

        public UserDTO Map(User entity)
        {
            var dto = LoadEntityData(entity);
            return dto;
        }

        public UserDTO Map(User entity,bool loadRole)
        {
            var dto = LoadEntityData(entity,loadRole);
            return dto;
        }

         private UserDTO LoadEntityData(User entity)
        {

            var myDto = BaseLoadEntity(entity);
            return myDto;
        }
         private UserDTO LoadEntityData(User entity,bool loadRole)
         {
             var myDto = BaseLoadEntity(entity);
             if (loadRole)
             {
                 //装载该用户角色
                 var userRoleMaps = _databaseContext.UserRoleMaps.Where(a => a.UserId == entity.Id);
                 foreach (var userRoleMap in userRoleMaps)
                 {
                     var role = _databaseContext.Roles.FirstOrDefault(a => a.Id == userRoleMap.RoleId);
                     if (role != null)
                     {
                         myDto.RoleDtos.Add(_roleMapper.Map(role));
                     }
                 }
             }
             return myDto;
         }

        private UserDTO BaseLoadEntity(User entity)
        {
            var myDto = new UserDTO()
                {
                    Id = entity.Id,
                    Name = entity.Name,
                    DepartmentId = entity.DepartmentId,
                    LoginName = entity.LoginName,
                    Code = entity.Code,
                    Remark = entity.Remark,
                    Password = entity.Password,
                    Position = entity.Position,
                    CreateDate = entity.CreateDate,
                    Status = entity.Status,
                    RoleDtos = new List<RoleDTO>()
                };

            if (entity.DepartmentId > 0)
            {
                var departments = _databaseContext.Departments.AsNoTracking().FirstOrDefault(a => a.Id == entity.DepartmentId);
                DepartmentDTO departmentsDto = null;
                if (departments != null)
                {
                    departmentsDto = new DepartmentDTO
                        {
                            Id = departments.Id,
                            Name = departments.Name
                        };
                }
                myDto.DepartmentEntity = departmentsDto;
            }
            return myDto;
        }
    }


}
