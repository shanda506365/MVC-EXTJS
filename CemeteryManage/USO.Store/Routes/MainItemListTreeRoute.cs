﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using USO.Mvc.Routes;

namespace USO.Store.Routes
{
    public class MainItemListTreeRoute : IRouteProvider
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
                                "LoadMainItemListTree",
                                new RouteValueDictionary
                                    {
                                        {"controller", "MainItemListTree"},
                                        {"action", "LoadMainItemListTree"}
                                    },
                                null,
                                null,
                                new MvcRouteHandler())
                        }
                };
        }
    }
}