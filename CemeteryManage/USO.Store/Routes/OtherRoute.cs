using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using USO.Mvc.Routes;

namespace USO.Store.Routes
{
    public class OtherRoute : IRouteProvider
    {
        public void GetRoutes(ICollection<RouteDescriptor> routes)
        {
            foreach (var routeDescriptor in GetRoutes())
                routes.Add(routeDescriptor);
        }

        public IEnumerable<RouteDescriptor> GetRoutes()
        {
            return new[]
                { 
                    //加载心跳
                      new RouteDescriptor
                        {
                            Priority = 20,
                            Route = new Route(
                                "Heartbeat",
                                new RouteValueDictionary
                                    {
                                        {"controller", "Other"},
                                        {"action", "Heartbeat"}
                                    },
                                null,
                                null,
                                new MvcRouteHandler())
                        }
              };
        }

    }
}