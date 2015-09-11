using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using USO.Mvc.Routes;

namespace USO.Store.Routes
{
    public class UserRoute : IRouteProvider
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
                     new RouteDescriptor
                        {
                            Priority = 20,
                            Route = new Route(
                                "",
                                new RouteValueDictionary
                                    {
                                        {"controller", "User"},
                                        {"action", "UserLogin"}
                                    },
                                null,
                                null,
                                new MvcRouteHandler())
                        },
                        //GetCurrUserRoleFun获取当前登录用户信息
                         new RouteDescriptor
                        {
                            Priority = 20,
                            Route = new Route(
                                "GetCurrUserInfo",
                                new RouteValueDictionary
                                    {
                                        {"controller", "User"},
                                        {"action", "GetCurrUserInfo"}
                                    },
                                null,
                                null,
                                new MvcRouteHandler())
                        },
                    //UserLogin
                    new RouteDescriptor
                        {
                            Priority = 20,
                            Route = new Route(
                                "UserLogin",
                                new RouteValueDictionary
                                    {
                                        {"controller", "User"},
                                        {"action", "UserLogin"}
                                    },
                                null,
                                null,
                                new MvcRouteHandler())
                        },
                        //CheckUserPassword
                         new RouteDescriptor
                        {
                            Priority = 20,
                            Route = new Route(
                                "CheckUserPassword",
                                new RouteValueDictionary
                                    {
                                        {"controller", "User"},
                                        {"action", "CheckUserPassword"}
                                    },
                                null,
                                null,
                                new MvcRouteHandler())
                        },
                        //UserLoginOut
                         new RouteDescriptor
                        {
                            Priority = 20,
                            Route = new Route(
                                "UserLoginOut",
                                new RouteValueDictionary
                                    {
                                        {"controller", "User"},
                                        {"action", "UserLoginOut"}
                                    },
                                null,
                                null,
                                new MvcRouteHandler())
                        }
                         //获取用户表信息
                        ,new RouteDescriptor
                        {
                            Priority = 20,
                            Route = new Route(
                                "LoadUserGrid",
                                new RouteValueDictionary
                                    {
                                        {"controller", "User"},
                                        {"action", "LoadUserGrid"}
                                    },
                                null,
                                null,
                                new MvcRouteHandler())
                        }
                        //添加用户
                         ,new RouteDescriptor
                        {
                            Priority = 20,
                            Route = new Route(
                                "AddUser",
                                new RouteValueDictionary
                                    {
                                        {"controller", "User"},
                                        {"action", "AddUser"}
                                    },
                                null,
                                null,
                                new MvcRouteHandler())
                        }
                        //编辑用户
                          ,new RouteDescriptor
                        {
                            Priority = 20,
                            Route = new Route(
                                "UpdateUser",
                                new RouteValueDictionary
                                    {
                                        {"controller", "User"},
                                        {"action", "UpdateUser"}
                                    },
                                null,
                                null,
                                new MvcRouteHandler())
                        }
                        //删除用户
                          ,new RouteDescriptor
                        {
                            Priority = 20,
                            Route = new Route(
                                "DelUser",
                                new RouteValueDictionary
                                    {
                                        {"controller", "User"},
                                        {"action", "DelUser"}
                                    },
                                null,
                                null,
                                new MvcRouteHandler())
                        }
                };
        }
    }
}