using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using USO.Mvc.Routes;
namespace USO.Store.Routes
{
    public class CemeteryAreaRoute : IRouteProvider
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
                      //获取墓碑区域表信息
                       new RouteDescriptor
                        {
                            Priority = 20,
                            Route = new Route(
                                "LoadCemeteryAreaGrid",
                                new RouteValueDictionary
                                    {
                                        {"controller", "CemeteryArea"},
                                        {"action", "LoadCemeteryAreaGrid"}
                                    },
                                null,
                                null,
                                new MvcRouteHandler())
                        }
                        //添加墓碑区域
                         ,new RouteDescriptor
                        {
                            Priority = 20,
                            Route = new Route(
                                "AddCemeteryArea",
                                new RouteValueDictionary
                                    {
                                        {"controller", "CemeteryArea"},
                                        {"action", "AddCemeteryArea"}
                                    },
                                null,
                                null,
                                new MvcRouteHandler())
                        }
                        //编辑墓碑区域
                          ,new RouteDescriptor
                        {
                            Priority = 20,
                            Route = new Route(
                                "UpdateCemeteryArea",
                                new RouteValueDictionary
                                    {
                                        {"controller", "CemeteryArea"},
                                        {"action", "UpdateCemeteryArea"}
                                    },
                                null,
                                null,
                                new MvcRouteHandler())
                        }
                        //删除墓碑区域
                          ,new RouteDescriptor
                        {
                            Priority = 20,
                            Route = new Route(
                                "DelCemeteryArea",
                                new RouteValueDictionary
                                    {
                                        {"controller", "CemeteryArea"},
                                        {"action", "DelCemeteryArea"}
                                    },
                                null,
                                null,
                                new MvcRouteHandler())
                        }
                };
        }
    }
}