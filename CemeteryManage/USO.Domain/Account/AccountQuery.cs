
namespace USO.Domain
{
    using System;
    using System.ComponentModel;
    using System.Linq;

    public class AccountQuery
    {
        public AccountQuery()
        {
            SortDirection = ListSortDirection.Ascending;
            PageSize = 50;
            Page = 1;
        }
    
        public ListSortDirection SortDirection { get; set; }
        public string SortMember { get; set; }
    
        //query.Skip((Page - 1) * PageSize);
        public int Page { get; set; }
        public int PageSize { get; set; }
        public bool IgnoreFilter { get; set; }
    
        public long? Id { get; set; }
        public bool FilterById { get { return Id.HasValue && Id.Value > 0; } }

        public string AccountName { get; set; }
        public bool FilterByAccountName {
            get { return !String.IsNullOrWhiteSpace(AccountName); }
        }

    }

    public static class AccountQueryableExtension
    {
        public static IQueryable<Account> Where(this IQueryable<Account> query, AccountQuery accountQuery)
        {
            bool hasFilter = false;

            if (accountQuery == null)
            {
                query = query.Where(u => false);
                return query;
            }

            if (accountQuery.FilterById)
            {
                query = query.Where(r => r.Id == accountQuery.Id);
                hasFilter = true;
            }

            if (accountQuery.FilterByAccountName)
            {
                query = query.Where(a => a.Name.Contains(accountQuery.AccountName));
                hasFilter = true;
            }


            if (!hasFilter && !accountQuery.IgnoreFilter)
            {
                query = query.Where(u => false);
            }

            return query;
        }
    }
}
