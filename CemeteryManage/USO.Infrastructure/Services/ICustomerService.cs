using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using USO.Core;
using USO.Domain;
using USO.Dto;

namespace USO.Infrastructure.Services
{
    public interface ICustomerService : IDependency
    {
        PagedResult<CustomerDTO> Find(CustomerQuery query);
    }
}
