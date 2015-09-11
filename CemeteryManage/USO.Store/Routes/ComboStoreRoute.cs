using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using USO.Mvc.Routes;

namespace USO.Store.Routes
{
    public class ComboStoreRoute : IRouteProvider
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
                    //加载角色 LoadRoleStore
                     new RouteDescriptor
                        {
                            Priority = 20,
                            Route = new Route(
                                "LoadRoleStore",
                                new RouteValueDictionary
                                    {
                                        {"controller", "ComboStore"},
                                        {"action", "LoadRoleStore"}
                                    },
                                null,
                                null,
                                new MvcRouteHandler())
                        },
                    //加载部门 LoadDepartmentStore
                      new RouteDescriptor
                        {
                            Priority = 20,
                            Route = new Route(
                                "LoadDepartmentStore",
                                new RouteValueDictionary
                                    {
                                        {"controller", "ComboStore"},
                                        {"action", "LoadDepartmentStore"}
                                    },
                                null,
                                null,
                                new MvcRouteHandler())
                        },
                    //加载付款状态 LoadPaymentStatusStore
                    new RouteDescriptor
                        {
                            Priority = 20,
                            Route = new Route(
                                "LoadPaymentStatusStore",
                                new RouteValueDictionary
                                    {
                                        {"controller", "ComboStore"},
                                        {"action", "LoadPaymentStatusStore"}
                                    },
                                null,
                                null,
                                new MvcRouteHandler())
                        },
                    //加载墓碑类别 LoadTombstoneTypeStore
                    new RouteDescriptor
                        {
                            Priority = 20,
                            Route = new Route(
                                "LoadTombstoneTypeStore",
                                new RouteValueDictionary
                                    {
                                        {"controller", "ComboStore"},
                                        {"action", "LoadTombstoneTypeStore"}
                                    },
                                null,
                                null,
                                new MvcRouteHandler())
                        },
                    //加载区域
                    new RouteDescriptor
                        {
                            Priority = 20,
                            Route = new Route(
                                "LoadAreaStore",
                                new RouteValueDictionary
                                    {
                                        {"controller", "ComboStore"},
                                        {"action", "LoadAreaStore"}
                                    },
                                null,
                                null,
                                new MvcRouteHandler())
                        },
                        //加载行
                    new RouteDescriptor
                        {
                            Priority = 20,
                            Route = new Route(
                                "LoadRowStore",
                                new RouteValueDictionary
                                    {
                                        {"controller", "ComboStore"},
                                        {"action", "LoadRowStore"}
                                    },
                                null,
                                null,
                                new MvcRouteHandler())
                        },
                        //加载列
                    new RouteDescriptor
                        {
                            Priority = 20,
                            Route = new Route(
                                "LoadColumnStore",
                                new RouteValueDictionary
                                    {
                                        {"controller", "ComboStore"},
                                        {"action", "LoadColumnStore"}
                                    },
                                null,
                                null,
                                new MvcRouteHandler())
                        },
                        //加载保密级别
                    new RouteDescriptor
                        {
                            Priority = 20,
                            Route = new Route(
                                "LoadSecurityLevelStore",
                                new RouteValueDictionary
                                    {
                                        {"controller", "ComboStore"},
                                        {"action", "LoadSecurityLevelStore"}
                                    },
                                null,
                                null,
                                new MvcRouteHandler())
                        },
                        //加载服务级别
                    new RouteDescriptor
                        {
                            Priority = 20,
                            Route = new Route(
                                "LoadServiceLevelStore",
                                new RouteValueDictionary
                                    {
                                        {"controller", "ComboStore"},
                                        {"action", "LoadServiceLevelStore"}
                                    },
                                null,
                                null,
                                new MvcRouteHandler())
                        },
                    //加载客户类型
                    new RouteDescriptor
                        {
                            Priority = 20,
                            Route = new Route(
                                "LoadCustomerTypeStore",
                                new RouteValueDictionary
                                    {
                                        {"controller", "ComboStore"},
                                        {"action", "LoadCustomerTypeStore"}
                                    },
                                null,
                                null,
                                new MvcRouteHandler())
                        },
                        //加载国籍
                    new RouteDescriptor
                        {
                            Priority = 20,
                            Route = new Route(
                                "LoadNationalityStore",
                                new RouteValueDictionary
                                    {
                                        {"controller", "ComboStore"},
                                        {"action", "LoadNationalityStore"}
                                    },
                                null,
                                null,
                                new MvcRouteHandler())
                        },
                        //加载客户状态
                    new RouteDescriptor
                        {
                            Priority = 20,
                            Route = new Route(
                                "LoadCustomerStatusStore",
                                new RouteValueDictionary
                                    {
                                        {"controller", "ComboStore"},
                                        {"action", "LoadCustomerStatusStore"}
                                    },
                                null,
                                null,
                                new MvcRouteHandler())
                        }
                        
                };
        }
    }
}