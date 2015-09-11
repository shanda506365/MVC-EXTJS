using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace USO.Domain
{
    public class CustomerQuery
    {
        public CustomerQuery()
        {
            dir = ListSortDirection.Ascending;
            sort = "Id";
            limit = 10;
            page = 1;
        }

        public ListSortDirection dir { get; set; }
        public string sort { get; set; }

        public int page { get; set; }
        public int limit { get; set; }

        public Customer filter { get; set; }

        public string BuryDateQuery { get; set; }

    }

    public static class CustomerQueryableExtension
    {
        public static IQueryable<Customer> Where(this IQueryable<Customer> query, CustomerQuery CustomerQuery)
        {
            if (CustomerQuery == null)
            {
                query = query.Where(u => false);
                return query;
            }
            if (CustomerQuery.filter == null)
            {
                return query;
            }

            if (CustomerQuery.filter.Id > 0)
            {
                query = query.Where(r => r.Id == CustomerQuery.filter.Id);
            }
            if (CustomerQuery.filter.CustomerTypeId > 0)
            {
                query = query.Where(r => r.CustomerTypeId == CustomerQuery.filter.CustomerTypeId);
            }
            if (CustomerQuery.filter.CustomerStatusId > 0)
            {
                query = query.Where(r => r.CustomerStatusId == CustomerQuery.filter.CustomerStatusId);
            }
            if (CustomerQuery.filter.NationalityId > 0)
            {
                query = query.Where(r => r.NationalityId == CustomerQuery.filter.NationalityId);
            }
            if (!string.IsNullOrEmpty(CustomerQuery.filter.FullName))
            {
                query = query.Where(r => (r.LastName + r.MiddleName + r.FirstName).Contains(CustomerQuery.filter.FullName));
            }
            if (!string.IsNullOrEmpty(CustomerQuery.BuryDateQuery))
            {
                var arry = CustomerQuery.BuryDateQuery.Split(',');
                var start = DateTime.Parse(arry[0]);
                var end = DateTime.Parse(arry[1]).AddDays(1);
                query = query.Where(r => r.BuryDate >= start && r.BuryDate <= end);
            }
            return query;
        }

    }
}
