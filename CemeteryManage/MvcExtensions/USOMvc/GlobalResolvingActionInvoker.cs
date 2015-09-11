using System.Linq;
using System.Collections.Generic;
using System.Web.Mvc;
using MvcExtensions;

namespace USO.Mvc.Filters
{
    /// <summary>
    /// 
    /// </summary>
    public class GlobalFilterResolvingActionInvoker : ExtendedControllerActionInvoker
    {
        private readonly IEnumerable<IGlobalFilterProvider> _filterProviders;


        /// <summary>
        /// 
        /// </summary>
        /// <param name="container"></param>
        ///// <param name="filterProviders"></param>
        public GlobalFilterResolvingActionInvoker(ContainerAdapter container)
            : base(container)
        {
            _filterProviders = container.GetServices<IGlobalFilterProvider>();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="controllerContext"></param>
        /// <param name="actionDescriptor"></param>
        /// <returns></returns>
        protected override FilterInfo GetFilters(ControllerContext controllerContext, ActionDescriptor actionDescriptor)
        {
            var filters = base.GetFilters(controllerContext, actionDescriptor);
            foreach (var provider in _filterProviders)
            {
                provider.AddFilters(filters);
            }
            return filters;
        }
    }
}