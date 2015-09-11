
namespace USO.Mvc.Routes
{
    using System.Collections.Generic;
    using USO.Core;

    public interface IRouteProvider : IDependency
    {
        /// <summary>
        /// obsolete, prefer other format for extension methods
        /// </summary>
        IEnumerable<RouteDescriptor> GetRoutes();

        void GetRoutes(ICollection<RouteDescriptor> routes);
    }
}