using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using USO.Core;
using USO.Domain;
using USO.Dto;

namespace USO.Infrastructure.Mappers
{
    public interface ISecurityLevelMapper : IDependency
    {
        SecurityLevelDTO Map(SecurityLevel entity);
    }

    public class SecurityLevelMapper : ISecurityLevelMapper
    {
        private readonly IDatabaseContext _databaseContext;

        public SecurityLevelMapper(IDatabaseContext databaseContext)
        {
            _databaseContext = databaseContext;
        }

        public SecurityLevelDTO Map(SecurityLevel entity)
        {
            var dto = LoadEntityData(entity);
            return dto;
        }

        private SecurityLevelDTO LoadEntityData(SecurityLevel entity)
        {

            var myDto = new SecurityLevelDTO()
            {
                Id = entity.Id,
                Name = entity.Name,
                Alias = entity.Alias,
                Remark = entity.Remark
            };


            return myDto;
        }
    }
}
