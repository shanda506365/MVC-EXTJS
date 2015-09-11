using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace USO.Dto
{
    public static class SortMemeberHelper
    {
        public static object GetPropertyValue(object obj, string property)
        {
            System.Reflection.PropertyInfo propertyInfo = obj.GetType().GetProperty(property);
            return propertyInfo.GetValue(obj, null);
        }

        public static Func<T, Tkey> DynamicLambda<T, Tkey>(string propertyName)
        {

            ParameterExpression p = Expression.Parameter(typeof(T), "p");
            Expression body = Expression.Property(p, typeof(T).GetProperty(propertyName));

            var lambda = Expression.Lambda<Func<T, Tkey>>(body, p);

            return lambda.Compile();
        }
        /// <summary>
        /// 分页方法
        /// </summary>
        /// <typeparam name="T">T</typeparam>
        /// <param name="source">IQueryable<T/></param>
        /// <param name="page">当前页数</param>
        /// <param name="limit">分页数</param>
        /// <returns>IQueryable<T/></returns>
        public static IQueryable<T> DataPaging<T>(IQueryable<T> source, int page, int limit)
        {
            if (page > 0)
            {
                return source.Skip((page - 1) * limit).Take(limit);
            }
            else
            {
                return source;
            }
            
        }
        /// <summary>
        /// 排序并分页
        /// </summary>
        /// <typeparam name="T">T</typeparam>
        /// <param name="source">IQueryable<T/></param>
        /// <param name="sortExpression">排序字段</param>
        /// <param name="sortDirection">排序方向</param>
        /// <param name="pageNumber">当前页数</param>
        /// <param name="pageSize">分页数</param>
        /// <returns>IQueryable<T/></returns>
        public static IQueryable<T> SortingAndPaging<T>(IQueryable<T> source, string sortExpression, ListSortDirection sortDirection, int pageNumber, int pageSize)
        {
            IQueryable<T> query = DataSorting<T>(source, sortExpression, sortDirection);
            return DataPaging(query, pageNumber, pageSize);
        }

        /// <summary>
        /// 排序
        /// </summary>
        /// <typeparam name="T">T</typeparam>
        /// <param name="source">IQueryable<T/></param>
        /// <param name="sortExpression">排序字段</param>
        /// <param name="sortDirection">排序方向</param>
        /// <returns>IQueryable<T/></returns>
        public static IQueryable<T> DataSorting<T>(IQueryable<T> source, string sortExpression, ListSortDirection sortDirection)
        {
            string sortingDir = sortDirection == ListSortDirection.Ascending ? "OrderBy" : "OrderByDescending";

            ParameterExpression param = Expression.Parameter(typeof(T), sortExpression);
            PropertyInfo pi = typeof(T).GetProperty(sortExpression);
            Type[] types = new Type[2];
            types[0] = typeof(T);
            types[1] = pi.PropertyType;
            Expression expr = Expression.Call(typeof(Queryable), sortingDir, types,
                source.Expression, Expression.Lambda(Expression.Property(param, sortExpression), param));
            IQueryable<T> query = source.AsQueryable().Provider.CreateQuery<T>(expr);
            return query;
        }
    }
}
