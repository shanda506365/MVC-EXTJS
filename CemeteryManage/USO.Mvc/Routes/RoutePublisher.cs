
namespace USO.Mvc.Routes
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Routing;

    public class RoutePublisher : IRoutePublisher
    {
        private readonly RouteCollection _routeCollection;
        private readonly Func<RouteBase, ShellRoute> _shellRouteFactory= r=>new ShellRoute(r);

        public RoutePublisher(
            RouteCollection routeCollection)
        {
            _routeCollection = routeCollection;
        }

        public void Publish(IEnumerable<RouteDescriptor> routes)
        {
            var routesArray = routes.OrderByDescending(r => r.Priority).ToArray();

            // this is not called often, but is intended to surface problems before
            // the actual collection is modified
            var preloading = new RouteCollection();
            foreach (var route in routesArray)
                preloading.Add(route.Name, route.Route);

            using (_routeCollection.GetWriteLock())
            {
                // existing routes are removed while the collection is briefly inaccessable
                var cropArray = _routeCollection
                    .OfType<ShellRoute>()
                    .ToArray();

                foreach (var crop in cropArray)
                {
                    _routeCollection.Remove(crop);
                }

                var urls = "<table>";
                // new routes are added
                foreach (var routeDescriptor in routesArray)
                {
                    var route=(Route)routeDescriptor.Route;
                    urls +=string.Format("<tr><td>{0}</td><td>{1}</td></tr>", route.Url, routeDescriptor.Priority) + Environment.NewLine;
                    _routeCollection.Add(routeDescriptor.Name, _shellRouteFactory(routeDescriptor.Route));
                }

                var sss = urls + "</table>";
            }
        }
    }
}