using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace USO.Domain
{
    public class TombstoneQuery
    {
        public TombstoneQuery()
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

        public Tombstone filter { get; set; }

        public string ExpiryDateQuery { get; set; }
        public string LastPaymentDateQuery { get; set; }
    }

    public static class TombstoneQueryableExtension
    {
        public static IQueryable<Tombstone> Where(this IQueryable<Tombstone> query, TombstoneQuery TombstoneQuery)
        {
            if (TombstoneQuery == null)
            {
                query = query.Where(u => false);
                return query;
            }

            if (TombstoneQuery.filter == null)
            {
                return query;
            }

            if (TombstoneQuery.filter.Id > 0)
            {
                query = query.Where(r => r.Id == TombstoneQuery.filter.Id);
            }
            if (TombstoneQuery.filter.AreaId > 0)
            {
                query = query.Where(r => r.AreaId == TombstoneQuery.filter.AreaId);
            }
            if (TombstoneQuery.filter.RowId > 0)
            {
                query = query.Where(r => r.RowId == TombstoneQuery.filter.RowId);
            }
            if (TombstoneQuery.filter.ColumnId > 0)
            {
                query = query.Where(r => r.ColumnId == TombstoneQuery.filter.ColumnId);
            }
            if (TombstoneQuery.filter.TypeId > 0)
            {
                query = query.Where(r => r.TypeId == TombstoneQuery.filter.TypeId);
            }
            if (TombstoneQuery.filter.PaymentStatusId > 0)
            {
                query = query.Where(r => r.PaymentStatusId == TombstoneQuery.filter.PaymentStatusId);
            }
            if (!string.IsNullOrEmpty(TombstoneQuery.filter.Name))
            {
                query = query.Where(r => r.Name.Contains(TombstoneQuery.filter.Name));
            }
            if (!string.IsNullOrEmpty(TombstoneQuery.filter.Alias))
            {
                query = query.Where(r => r.Alias.Contains(TombstoneQuery.filter.Alias));
            }

            if (!string.IsNullOrEmpty(TombstoneQuery.ExpiryDateQuery))
            {
                var arry = TombstoneQuery.ExpiryDateQuery.Split(',');
                var start = DateTime.Parse(arry[0]);
                var end = DateTime.Parse(arry[1]).AddDays(1);
                query = query.Where(r => r.ExpiryDate >= start && r.ExpiryDate<=end);
            }
            //补交日期
            if (!string.IsNullOrEmpty(TombstoneQuery.LastPaymentDateQuery))
            {
                var arry = TombstoneQuery.LastPaymentDateQuery.Split(',');
                var start = DateTime.Parse(arry[0]);
                var end = DateTime.Parse(arry[1]).AddDays(1);
                query = query.Where(r => r.LastPaymentDate >= start && r.LastPaymentDate <= end);
            }
            
            return query;
        }

    }
}
