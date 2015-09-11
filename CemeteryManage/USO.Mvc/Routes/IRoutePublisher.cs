
namespace USO.Mvc.Routes
{
    using System.Collections.Generic;
    using USO.Core;

    public interface IRoutePublisher : IDependency
    {
        void Publish(IEnumerable<RouteDescriptor> routes);
    }
}