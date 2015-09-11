using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using USO.Mvc.Routes;


namespace USO.Store.Routes
{
    public class RoleRoute : IRouteProvider
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
                    //获取相应的功能
                    new RouteDescriptor
                        {
                            Priority = 20,
                            Route = new Route(
                                "LoadFunctions",
                                new RouteValueDictionary
                                    {
                                        {"controller", "Role"},
                                        {"action", "LoadFunctions"}
                                    },
                                null,
                                null,
                                new MvcRouteHandler())
                        },
                      //获取角色表信息
                       new RouteDescriptor
                        {
                            Priority = 20,
                            Route = new Route(
                                "LoadRoleGrid",
                                new RouteValueDictionary
                                    {
                                        {"controller", "Role"},
                                        {"action", "LoadRoleGrid"}
                                    },
                                null,
                                null,
                                new MvcRouteHandler())
                        }
                        //添加角色
                         ,new RouteDescriptor
                        {
                            Priority = 20,
                            Route = new Route(
                                "AddRole",
                                new RouteValueDictionary
                                    {
                                        {"controller", "Role"},
                                        {"action", "AddRole"}
                                    },
                                null,
                                null,
                                new MvcRouteHandler())
                        }
                        //编辑角色
                          ,new RouteDescriptor
                        {
                            Priority = 20,
                            Route = new Route(
                                "UpdateRole",
                                new RouteValueDictionary
                                    {
                                        {"controller", "Role"},
                                        {"action", "UpdateRole"}
                                    },
                                null,
                                null,
                                new MvcRouteHandler())
                        }
                        //删除角色
                          ,new RouteDescriptor
                        {
                            Priority = 20,
                            Route = new Route(
                                "DelRole",
                                new RouteValueDictionary
                                    {
                                        {"controller", "Role"},
                                        {"action", "DelRole"}
                                    },
                                null,
                                null,
                                new MvcRouteHandler())
                        }
                };
        }
    }
}