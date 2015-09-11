using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using USO.Mvc.Routes;

namespace USO.Store.Routes
{
    public class CustomerRoute : IRouteProvider
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
                                "Index",
                                new RouteValueDictionary
                                    {
                                        {"controller", "Customer"},
                                        {"action", "Index"}
                                    },
                                null,
                                null,
                                new MvcRouteHandler())
                        }
                        //获取客户表信息
                        ,new RouteDescriptor
                        {
                            Priority = 20,
                            Route = new Route(
                                "LoadCustomerGrid",
                                new RouteValueDictionary
                                    {
                                        {"controller", "Customer"},
                                        {"action", "LoadCustomerGrid"}
                                    },
                                null,
                                null,
                                new MvcRouteHandler())
                        }
                        //添加客户
                         ,new RouteDescriptor
                        {
                            Priority = 20,
                            Route = new Route(
                                "AddCustomer",
                                new RouteValueDictionary
                                    {
                                        {"controller", "Customer"},
                                        {"action", "AddCustomer"}
                                    },
                                null,
                                null,
                                new MvcRouteHandler())
                        }
                        //编辑客户
                          ,new RouteDescriptor
                        {
                            Priority = 20,
                            Route = new Route(
                                "UpdateCustomer",
                                new RouteValueDictionary
                                    {
                                        {"controller", "Customer"},
                                        {"action", "UpdateCustomer"}
                                    },
                                null,
                                null,
                                new MvcRouteHandler())
                        }
                        //删除客户
                          ,new RouteDescriptor
                        {
                            Priority = 20,
                            Route = new Route(
                                "DelCustomer",
                                new RouteValueDictionary
                                    {
                                        {"controller", "Customer"},
                                        {"action", "DelCustomer"}
                                    },
                                null,
                                null,
                                new MvcRouteHandler())
                        }
                        //导出客户
                         ,new RouteDescriptor
                        {
                            Priority = 20,
                            Route = new Route(
                                "OutPutExcelCustomer",
                                new RouteValueDictionary
                                    {
                                        {"controller", "Customer"},
                                        {"action", "OutPutExcelCustomer"}
                                    },
                                null,
                                null,
                                new MvcRouteHandler())
                        }
                        //初始化客户导入
                          ,new RouteDescriptor
                        {
                            Priority = 20,
                            Route = new Route(
                                "UploadCustomerExcel",
                                new RouteValueDictionary
                                    {
                                        {"controller", "Customer"},
                                        {"action", "UploadCustomerExcel"}
                                    },
                                null,
                                null,
                                new MvcRouteHandler())
                        }
                        //导入客户
                         ,new RouteDescriptor
                        {
                            Priority = 20,
                            Route = new Route(
                                "InputCustomerStocksFromInput",
                                new RouteValueDictionary
                                    {
                                        {"controller", "Customer"},
                                        {"action", "InputCustomerStocksFromInput"}
                                    },
                                null,
                                null,
                                new MvcRouteHandler())
                        }
                        
                        
                };
        }
    }
}