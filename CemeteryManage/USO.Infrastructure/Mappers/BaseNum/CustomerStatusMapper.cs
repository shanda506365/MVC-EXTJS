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
    public interface ICustomerStatusMapper : IDependency
    {
        CustomerStatusDTO Map(CustomerStatus entity);
    }

    public class CustomerStatusMapper : ICustomerStatusMapper
    {
        private readonly IDatabaseContext _databaseContext;

        public CustomerStatusMapper(IDatabaseContext databaseContext)
        {
            _databaseContext = databaseContext;
        }

        public CustomerStatusDTO Map(CustomerStatus entity)
        {
            var dto = LoadEntityData(entity);
            return dto;
        }

        private CustomerStatusDTO LoadEntityData(CustomerStatus entity)
        {

            var myDto = new CustomerStatusDTO()
            {
                Id = entity.Id,
                Name = entity.Name
            };


            return myDto;
        }
    }
}
