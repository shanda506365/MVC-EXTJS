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
    public interface ICustomerMapper : IDependency
    {
        CustomerDTO Map(Customer entity);
    }

    public class CustomerMapper : ICustomerMapper
    {
        private readonly IDatabaseContext _databaseContext;

        public CustomerMapper(IDatabaseContext databaseContext)
        {
            _databaseContext = databaseContext;
        }

        public CustomerDTO Map(Customer entity)
        {
            var dto = LoadEntityData(entity);
            return dto;
        }

        private CustomerDTO LoadEntityData(Customer entity)
        {

            var myDto = new CustomerDTO()
            {
                Id=entity.Id,
                FullName = entity.FullName,
                LastName = entity.LastName,
                FirstName = entity.FirstName,
                MiddleName = entity.MiddleName,
                Remark = entity.Remark,
                Telephone = entity.Telephone,
                Phone = entity.Phone,
                OtherPhone = entity.OtherPhone,
                Address = entity.Address,
                CustomerTypeId = entity.CustomerTypeId,
                LinkCustomerId = entity.LinkCustomerId,
                BuryDate = entity.BuryDate,
                DeathDate = entity.DeathDate,
                CustomerStatusId = entity.CustomerStatusId,
                IDNumber = entity.IDNumber
            };
            
            return myDto;
        }
    }
}
