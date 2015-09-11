using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace USO.Domain
{
    public class FunctionQuery
    {
        public FunctionQuery()
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

        public Function filter { get; set; }
    }

    public static class FunctionQueryableExtension
    {
        public static IQueryable<Function> Where(this IQueryable<Function> query, FunctionQuery functionQuery)
        {
            if (functionQuery == null)
            {
                query = query.Where(u => false);
                return query;
            }

            if (functionQuery.filter == null)
            {
                return query;
            }

            if (functionQuery.filter.Id > 0)
            {
                query = query.Where(r => r.Id == functionQuery.filter.Id);
            }
            if (!string.IsNullOrEmpty(functionQuery.filter.Name))
            {
                query = query.Where(r => r.Name.Contains(functionQuery.filter.Name));
            }
            if (!string.IsNullOrEmpty(functionQuery.filter.Code))
            {
                query = query.Where(r => r.Code.Contains(functionQuery.filter.Code));
            }
            if (functionQuery.filter.ParentId > 0)
            {
                query = query.Where(r => r.ParentId == functionQuery.filter.ParentId);
            }
            return query;
        }

    }
}
