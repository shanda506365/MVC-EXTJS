using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace USO.Domain
{
    public class UserQuery
    {
        public UserQuery()
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

        public User filter { get; set; }

        public string CreateDateQuery { get; set; }

    }

    public static class UserQueryableExtension
    {
        public static IQueryable<User> Where(this IQueryable<User> query, UserQuery UserQuery)
        {
            if (UserQuery == null)
            {
                query = query.Where(u => false);
                return query;
            }

            if (UserQuery.filter == null)
            {
                return query;
            }

            if (UserQuery.filter.Id > 0)
            {
                query = query.Where(r => r.Id == UserQuery.filter.Id);
            }
            if (!string.IsNullOrEmpty(UserQuery.filter.Name))
            {
                query = query.Where(r => r.Name.Contains(UserQuery.filter.Name));
            }
            if (UserQuery.filter.DepartmentId > 0)
            {
                query = query.Where(r => r.DepartmentId == UserQuery.filter.DepartmentId);
            }
            if (!string.IsNullOrEmpty(UserQuery.filter.LoginName))
            {
                query = query.Where(r => r.LoginName.Contains(UserQuery.filter.LoginName));
            }
            if (!string.IsNullOrEmpty(UserQuery.filter.Code))
            {
                query = query.Where(r => r.Code.Contains(UserQuery.filter.Code));
            }
            if (!string.IsNullOrEmpty(UserQuery.filter.Position))
            {
                query = query.Where(r => r.Position.Contains(UserQuery.filter.Position));
            }
            if (UserQuery.filter.Status != null)
            {
                query = query.Where(r => r.Status == UserQuery.filter.Status);
            }

            if (!string.IsNullOrEmpty(UserQuery.CreateDateQuery))
            {
                var arry = UserQuery.CreateDateQuery.Split(',');
                var start = DateTime.Parse(arry[0]);
                var end = DateTime.Parse(arry[1]).AddDays(1);
                query = query.Where(r => r.CreateDate >= start && r.CreateDate <= end);
            }
            return query;
        }

    }
}
