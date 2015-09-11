using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using USO.Mvc.Routes;

namespace USO.Store.Routes
{
    public class ExReportListTreeRoute : IRouteProvider
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
                    //LoadMainItemListTree
                    new RouteDescriptor
                        {
                            Priority = 20,
                            Route = new Route(
                                "LoadExReportListTree",
                                new RouteValueDictionary
                                    {
                                        {"controller", "ExReportListTree"},
                                        {"action", "LoadExReportListTree"}
                                    },
                                null,
                                null,
                                new MvcRouteHandler())
                        }
                };
        }
    }
}