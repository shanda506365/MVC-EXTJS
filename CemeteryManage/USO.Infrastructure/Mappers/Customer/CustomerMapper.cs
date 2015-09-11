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
        CustomerDTO Map(Customer entity, bool includeAll);
    }

    public class CustomerMapper : ICustomerMapper
    {
        private readonly IDatabaseContext _databaseContext;

        public CustomerMapper(IDatabaseContext databaseContext)
        {
            _databaseContext = databaseContext;
        }

        public CustomerDTO Map(Customer entity, bool includeAll)
        {
            var dto = LoadEntityData(entity, includeAll);
            return dto;
        }

        private CustomerDTO LoadEntityData(Customer entity, bool includeAll)
        {

            var myDto = new CustomerDTO()
            {
                Id=entity.Id,
                FullName = string.IsNullOrEmpty(entity.FullName)? entity.LastName+entity.MiddleName+ entity.FirstName:entity.FullName,
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
                NationalityId = entity.NationalityId,
                BuryDate = entity.BuryDate,
                DeathDate = entity.DeathDate,
                CustomerStatusId = entity.CustomerStatusId,
                IDNumber = entity.IDNumber
            };
            //包括所有关系
            if (includeAll)
            {
                if (entity.CustomerTypeId.HasValue&& entity.CustomerTypeId.Value > 0)
                {
                    var customerType = _databaseContext.CustomerTypes.AsNoTracking().FirstOrDefault(a=>a.Id==entity.CustomerTypeId);
                    myDto.CustomerTypeName = customerType==null?"":customerType.Name;

                }
                if (entity.LinkCustomerId.HasValue && entity.LinkCustomerId.Value > 0)
                {
                    var linkCustomer =
                        _databaseContext.Customers.AsNoTracking().FirstOrDefault(a => a.Id == entity.LinkCustomerId);
                    myDto.LinkCustomerName = linkCustomer == null ? "" : entity.LastName+entity.MiddleName+ entity.FirstName;
                }
                if (entity.CustomerStatusId.HasValue && entity.CustomerStatusId.Value > 0)
                {
                    var customerStatus = _databaseContext.CustomerStatus.AsNoTracking().FirstOrDefault(a=>a.Id==entity.CustomerStatusId);
                    myDto.CustomerStatusName = customerStatus==null?"":customerStatus.Name;
                }
                if (entity.NationalityId.HasValue && entity.NationalityId.Value > 0)
                {
                    var nationality = _databaseContext.Nationalitys.AsNoTracking().FirstOrDefault(a => a.Id == entity.NationalityId);
                    myDto.NationalityName = nationality == null ? "" : nationality.Name;
                }
            }
            
            return myDto;
        }
    }
}
