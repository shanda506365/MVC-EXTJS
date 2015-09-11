using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace USO.Domain
{
    public class RoleQuery
    {
        public RoleQuery()
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

        public Role filter { get; set; }

        public string CreateDateQuery { get; set; }
    }

    public static class RoleQueryableExtension
    {
        public static IQueryable<Role> Where(this IQueryable<Role> query, RoleQuery roleQuery)
        {
            if (roleQuery == null)
            {
                query = query.Where(u => false);
                return query;
            }

            if (roleQuery.filter == null)
            {
                return query;
            }

            if (roleQuery.filter.Id > 0)
            {
                query = query.Where(r => r.Id == roleQuery.filter.Id);
            }
            if (!string.IsNullOrEmpty(roleQuery.filter.Name))
            {
                query = query.Where(r => r.Name.Contains(roleQuery.filter.Name));
            }
            return query;
        }

    }
}
