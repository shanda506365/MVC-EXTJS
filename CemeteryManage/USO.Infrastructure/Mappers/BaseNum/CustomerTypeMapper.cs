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
    public interface ICustomerTypeMapper : IDependency
    {
        CustomerTypeDTO Map(CustomerType entity);
    }

    public class CustomerTypeMapper : ICustomerTypeMapper
    {
        private readonly IDatabaseContext _databaseContext;

        public CustomerTypeMapper(IDatabaseContext databaseContext)
        {
            _databaseContext = databaseContext;
        }

        public CustomerTypeDTO Map(CustomerType entity)
        {
            var dto = LoadEntityData(entity);
            return dto;
        }

        private CustomerTypeDTO LoadEntityData(CustomerType entity)
        {

            var myDto = new CustomerTypeDTO()
            {
                Id = entity.Id,
                Name = entity.Name,
                Description = entity.Description
            };
            

            return myDto;
        }
    }
}
