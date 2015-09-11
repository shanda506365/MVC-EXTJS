using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using USO.Mvc.Routes;

namespace USO.Store.Routes
{
    public class SysLogRoute : IRouteProvider
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
                    
                         //获取日志表信息
                        new RouteDescriptor
                        {
                            Priority = 20,
                            Route = new Route(
                                "LoadSysLogGrid",
                                new RouteValueDictionary
                                    {
                                        {"controller", "SysLog"},
                                        {"action", "LoadSysLogGrid"}
                                    },
                                null,
                                null,
                                new MvcRouteHandler())
                        }
                        //添加日志
                         ,new RouteDescriptor
                        {
                            Priority = 20,
                            Route = new Route(
                                "AddSysLog",
                                new RouteValueDictionary
                                    {
                                        {"controller", "SysLog"},
                                        {"action", "AddSysLog"}
                                    },
                                null,
                                null,
                                new MvcRouteHandler())
                        }
                       
                };
        }
    }
}