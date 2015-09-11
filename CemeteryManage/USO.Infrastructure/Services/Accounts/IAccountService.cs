
using USO.Dto.Accounts;

namespace USO.Infrastructure.Accounts
{
    using System.Collections.Generic;
    using USO.Core;
    using USO.Domain;
    using USO.Dto;
    using USO.Dto.Activities;

    public interface IAccountService : IDependency
    {
        PagedResult<AccountDTO> Find(AccountQuery query);
    }
}
