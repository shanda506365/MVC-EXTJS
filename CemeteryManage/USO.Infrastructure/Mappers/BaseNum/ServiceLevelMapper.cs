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
    public interface IServiceLevelMapper : IDependency
    {
        ServiceLevelDTO Map(ServiceLevel entity);
    }

    public class ServiceLevelMapper : IServiceLevelMapper
    {
        private readonly IDatabaseContext _databaseContext;

        public ServiceLevelMapper(IDatabaseContext databaseContext)
        {
            _databaseContext = databaseContext;
        }

        public ServiceLevelDTO Map(ServiceLevel entity)
        {
            var dto = LoadEntityData(entity);
            return dto;
        }

        private ServiceLevelDTO LoadEntityData(ServiceLevel entity)
        {

            var myDto = new ServiceLevelDTO()
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
