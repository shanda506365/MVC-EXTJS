using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace USO.Domain
{
    public class CemeteryAreasQuery
    {
        public CemeteryAreasQuery()
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

        public CemeteryAreas filter { get; set; }
    }
    public static class CemeteryAreasQueryableExtension
    {
        public static IQueryable<CemeteryAreas> Where(this IQueryable<CemeteryAreas> query, CemeteryAreasQuery CemeteryAreasQuery)
        {
            if (CemeteryAreasQuery == null)
            {
                query = query.Where(u => false);
                return query;
            }
            if (CemeteryAreasQuery.filter == null)
            {
                return query;
            }

            if (CemeteryAreasQuery.filter.Id > 0)
            {
                query = query.Where(r => r.Id == CemeteryAreasQuery.filter.Id);
            }

            if (!string.IsNullOrEmpty(CemeteryAreasQuery.filter.Name))
            {
                query = query.Where(r => r.Name.Contains(CemeteryAreasQuery.filter.Name));
            }
            if (!string.IsNullOrEmpty(CemeteryAreasQuery.filter.Alias))
            {
                query = query.Where(r => r.Alias == CemeteryAreasQuery.filter.Alias);
            }

            return query;
        }

    }
}
