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
    public interface IRoleMapper : IDependency
    {
        RoleDTO Map(Role entity);
    }

    public class RoleMapper : IRoleMapper
    {
        private readonly IDatabaseContext _databaseContext;

        public RoleMapper(IDatabaseContext databaseContext)
        {
            _databaseContext = databaseContext;
        }

        public RoleDTO Map(Role entity)
        {
            var dto = LoadEntityData(entity);
            return dto;
        }

        private RoleDTO LoadEntityData(Role entity)
        {

            var myDto = new RoleDTO()
            {
                Id = entity.Id,
                Name = entity.Name,
                FunctionDtos = new List<FunctionDTO>()
            };
            //装载该角色功能
            var roleFunctionMaps = _databaseContext.RoleFunctionMaps.Where(a => a.RoleId == myDto.Id);
            foreach (var roleFunctionMap in roleFunctionMaps)
            {
                var functions = _databaseContext.Functions.FirstOrDefault(a => a.Id == roleFunctionMap.FunctionId);
                if (functions != null)
                {
                    var functionDto = new FunctionDTO
                    {
                        Id = functions.Id,
                        Code = functions.Code,
                        Name = functions.Name,
                        ParentId = functions.ParentId,
                        Url = functions.Url
                    };
                    myDto.FunctionDtos.Add(functionDto);
                }
            }
           


            return myDto;
        }
    }
}
