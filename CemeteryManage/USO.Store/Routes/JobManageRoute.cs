using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using USO.Mvc.Routes;
namespace USO.Store.Routes
{
    public class JobManageRoute : IRouteProvider
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
                     //预订墓碑
                      new RouteDescriptor
                        {
                            Priority = 20,
                            Route = new Route(
                                "OrderTombstone",
                                new RouteValueDictionary
                                    {
                                        {"controller", "JobManage"},
                                        {"action", "OrderTombstone"}
                                    },
                                null,
                                null,
                                new MvcRouteHandler())
                        },
                        //维护墓碑
                      new RouteDescriptor
                        {
                            Priority = 20,
                            Route = new Route(
                                "MaintainTombstone",
                                new RouteValueDictionary
                                    {
                                        {"controller", "JobManage"},
                                        {"action", "MaintainTombstone"}
                                    },
                                null,
                                null,
                                new MvcRouteHandler())
                        },
                        //落葬墓碑
                      new RouteDescriptor
                        {
                            Priority = 20,
                            Route = new Route(
                                "BuryTombstone",
                                new RouteValueDictionary
                                    {
                                        {"controller", "JobManage"},
                                        {"action", "BuryTombstone"}
                                    },
                                null,
                                null,
                                new MvcRouteHandler())
                        },
                        //落葬编辑
                         new RouteDescriptor
                        {
                            Priority = 20,
                            Route = new Route(
                                "UpdateBuryMan",
                                new RouteValueDictionary
                                    {
                                        {"controller", "JobManage"},
                                        {"action", "UpdateBuryMan"}
                                    },
                                null,
                                null,
                                new MvcRouteHandler())
                        },
                        //退订 复位墓碑
                         new RouteDescriptor
                        {
                            Priority = 20,
                            Route = new Route(
                                "ClearTombstone",
                                new RouteValueDictionary
                                    {
                                        {"controller", "JobManage"},
                                        {"action", "ClearTombstone"}
                                    },
                                null,
                                null,
                                new MvcRouteHandler())
                        },
                        //查询已订墓碑相关业务信息
                         new RouteDescriptor
                        {
                            Priority = 20,
                            Route = new Route(
                                "GetOrderJobInfoTombstone",
                                new RouteValueDictionary
                                    {
                                        {"controller", "JobManage"},
                                        {"action", "GetOrderJobInfoTombstone"}
                                    },
                                null,
                                null,
                                new MvcRouteHandler())
                        },
                        //查询 墓碑相关业务信息 日志
                         new RouteDescriptor
                        {
                            Priority = 20,
                            Route = new Route(
                                "GetTombstoneJobInfoLog",
                                new RouteValueDictionary
                                    {
                                        {"controller", "JobManage"},
                                        {"action", "GetTombstoneJobInfoLog"}
                                    },
                                null,
                                null,
                                new MvcRouteHandler())
                        },
                        //修改申请人 EditApplicanter
                         new RouteDescriptor
                        {
                            Priority = 20,
                            Route = new Route(
                                "EditApplicanter",
                                new RouteValueDictionary
                                    {
                                        {"controller", "JobManage"},
                                        {"action", "EditApplicanter"}
                                    },
                                null,
                                null,
                                new MvcRouteHandler())
                        },
                        //续交管理费
                          new RouteDescriptor
                        {
                            Priority = 20,
                            Route = new Route(
                                "RenewManageLimit",
                                new RouteValueDictionary
                                    {
                                        {"controller", "JobManage"},
                                        {"action", "RenewManageLimit"}
                                    },
                                null,
                                null,
                                new MvcRouteHandler())
                        },
                        //获取续交管理费记录 GetTombstoneRenewManangeLog
                           new RouteDescriptor
                        {
                            Priority = 20,
                            Route = new Route(
                                "GetTombstoneRenewManangeLog",
                                new RouteValueDictionary
                                    {
                                        {"controller", "JobManage"},
                                        {"action", "GetTombstoneRenewManangeLog"}
                                    },
                                null,
                                null,
                                new MvcRouteHandler())
                        }
                };
        }
    }
}