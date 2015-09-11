using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using USO.Domain;
using USO.Dto;
using USO.Infrastructure.Mappers;

namespace USO.Infrastructure.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly IDatabaseContext _databaseContext;
        private readonly CustomerMapper _customerMapper;

        public CustomerService(IDatabaseContext databaseContext, CustomerMapper customerMapper)
        {
            _databaseContext = databaseContext;
            _customerMapper = customerMapper;
        }

        public PagedResult<CustomerDTO> Find(CustomerQuery customerQuery)
        {
            Check.Argument.IsNotNull(customerQuery, "customerQuery");

            //Apply filtering    

            var query = _databaseContext.Customers.Where(customerQuery);
            var total = query.Count();
            query = customerQuery.SortDirection == ListSortDirection.Ascending
                        ? query.OrderBy(m => customerQuery.SortMember)
                        : query.OrderByDescending(m => customerQuery.SortMember);

            if (customerQuery.PageSize > 0)
            {
                query = query.Skip((customerQuery.Page - 1) * customerQuery.PageSize);
            }
            query = query.Take(customerQuery.PageSize);

            var resultSet = query.AsNoTracking().ToList().Select(r => _customerMapper.Map(r)).ToList();

            return new PagedResult<CustomerDTO>(resultSet, total);
        }
    }
}
