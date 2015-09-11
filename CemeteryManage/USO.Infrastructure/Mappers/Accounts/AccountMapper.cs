using USO.Dto.Accounts;

namespace USO.Infrastructure.Mappers
{
    using System.Data.Entity;
    using System.Linq;
    using USO.Core;
    using USO.Domain;
    using USO.Dto.Activities;
    using USO.Infrastructure.Security;

    public interface IAccountMapper : IDependency 
    {
        AccountDTO Map(Account entity);

        AccountDTO MapSimple(Account entity);
    }
    
    public class AccountMapper : IAccountMapper
    {
        private readonly WorkContext _workContext;
        private readonly IDatabaseContext _databaseContext;
        private readonly IMembershipService _membershipService;

        public AccountMapper(
            WorkContext workContext,
            IDatabaseContext databaseContext,
            IMembershipService membershipService
            )
        {
            _workContext = workContext;
            _databaseContext = databaseContext;
            _membershipService = membershipService;
        }

        public AccountDTO Map(Account entity)
        {
            var dto = LoadEntityData(entity);
            return dto;
    	 }

        public AccountDTO MapSimple(Account entity)
        {
            var dto = LoadEntityData(entity);
           
            return dto;
        }

        private AccountDTO LoadEntityData(Account entity)
        {
            return new AccountDTO
                          {
                                Id = entity.Id,
                                Name=entity.Name,
                                R3Code = entity.R3Code,
                                Description=entity.Description,
                                CreatedBy = entity.CreatedBy,
                                CreatedOn = entity.CreatedOn,
                                ModifiedBy = entity.ModifiedBy,
                                ModifiedOn = entity.ModifiedOn
                                
                          };
        }
    }
}
