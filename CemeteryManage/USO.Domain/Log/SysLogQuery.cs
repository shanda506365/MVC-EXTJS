using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace USO.Domain
{
    public  class SysLogQuery
    {
        public SysLogQuery()
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

        public SysLog filter { get; set; }

        public string Date { get; set; }

    }

    public static class SysLogQueryableExtension
    {
        public static IQueryable<SysLog> Where(this IQueryable<SysLog> query, SysLogQuery sysLogQuery)
        {
            if (sysLogQuery == null)
            {
                query = query.Where(u => false);
                return query;
            }

            if (sysLogQuery.filter == null)
            {
                return query;
            }

            if (sysLogQuery.filter.Id > 0)
            {
                query = query.Where(r => r.Id == sysLogQuery.filter.Id);
            }
            if (sysLogQuery.filter.Type > 0)
            {
                query = query.Where(r => r.Type == sysLogQuery.filter.Type);
            }
            if (!string.IsNullOrEmpty(sysLogQuery.filter.ControlName))
            {
                query = query.Where(r => r.ControlName.Contains(sysLogQuery.filter.ControlName));
            }
            if (sysLogQuery.filter.UserId > 0)
            {
                query = query.Where(r => r.UserId == sysLogQuery.filter.UserId);
            }
            if (sysLogQuery.filter.ControllTid > 0)
            {
                query = query.Where(r => r.ControllTid == sysLogQuery.filter.ControllTid);
            }
            if (sysLogQuery.filter.Type > 0)
            {
                query = query.Where(r => r.Type == sysLogQuery.filter.Type);
            }


            if (!string.IsNullOrEmpty(sysLogQuery.Date))
            {
                var arry = sysLogQuery.Date.Split(',');
                var start = DateTime.Parse(arry[0]);
                var end = DateTime.Parse(arry[1]).AddDays(1);
                query = query.Where(r => r.Date >= start && r.Date <= end);
            }
            return query;
        }

    }
}
