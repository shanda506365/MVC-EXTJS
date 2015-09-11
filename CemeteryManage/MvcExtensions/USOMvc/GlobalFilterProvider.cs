using System.Web.Mvc;

namespace USO.Mvc.Filters
{
    /// <summary>
    /// 
    /// </summary>
    public interface IGlobalFilterProvider
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="filterInfo"></param>
        void AddFilters(FilterInfo filterInfo);
    }

    /// <summary>
    /// 
    /// </summary>
    public abstract class GlobalFilterProvider : IGlobalFilterProvider
    {
        void IGlobalFilterProvider.AddFilters(FilterInfo filterInfo)
        {
            AddFilters(filterInfo);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="filterInfo"></param>
        protected virtual void AddFilters(FilterInfo filterInfo)
        {
            if (this is IAuthorizationFilter)
                filterInfo.AuthorizationFilters.Add(this as IAuthorizationFilter);
            if (this is IActionFilter)
                filterInfo.ActionFilters.Add(this as IActionFilter);
            if (this is IResultFilter)
                filterInfo.ResultFilters.Add(this as IResultFilter);
            if (this is IExceptionFilter)
                filterInfo.ExceptionFilters.Add(this as IExceptionFilter);
        }

    }
}
