using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using USO.Mvc.Routes;

namespace USO.Store.Routes
{
    public class TombstoneRoute : IRouteProvider
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
                   //获取墓碑表信息
                       new RouteDescriptor
                        {
                            Priority = 20,
                            Route = new Route(
                                "LoadTombstoneGrid",
                                new RouteValueDictionary
                                    {
                                        {"controller", "Tombstone"},
                                        {"action", "LoadTombstoneGrid"}
                                    },
                                null,
                                null,
                                new MvcRouteHandler())
                        }
                        //添加墓碑
                         ,new RouteDescriptor
                        {
                            Priority = 20,
                            Route = new Route(
                                "AddTombstone",
                                new RouteValueDictionary
                                    {
                                        {"controller", "Tombstone"},
                                        {"action", "AddTombstone"}
                                    },
                                null,
                                null,
                                new MvcRouteHandler())
                        }
                        //批量添加 AddTombstoneRowList
                         ,new RouteDescriptor
                        {
                            Priority = 20,
                            Route = new Route(
                                "AddTombstoneRowList",
                                new RouteValueDictionary
                                    {
                                        {"controller", "Tombstone"},
                                        {"action", "AddTombstoneRowList"}
                                    },
                                null,
                                null,
                                new MvcRouteHandler())
                        }
                        //编辑墓碑
                          ,new RouteDescriptor
                        {
                            Priority = 20,
                            Route = new Route(
                                "UpdateTombstone",
                                new RouteValueDictionary
                                    {
                                        {"controller", "Tombstone"},
                                        {"action", "UpdateTombstone"}
                                    },
                                null,
                                null,
                                new MvcRouteHandler())
                        }
                        //删除墓碑
                          ,new RouteDescriptor
                        {
                            Priority = 20,
                            Route = new Route(
                                "DelTombstone",
                                new RouteValueDictionary
                                    {
                                        {"controller", "Tombstone"},
                                        {"action", "DelTombstone"}
                                    },
                                null,
                                null,
                                new MvcRouteHandler())
                        }
                        //墓碑排序
                          ,new RouteDescriptor
                        {
                            Priority = 20,
                            Route = new Route(
                                "SortTombstonePng",
                                new RouteValueDictionary
                                    {
                                        {"controller", "Tombstone"},
                                        {"action", "SortTombstonePng"}
                                    },
                                null,
                                null,
                                new MvcRouteHandler())
                        }
                        //墓碑导出
                         ,new RouteDescriptor
                        {
                            Priority = 20,
                            Route = new Route(
                                "OutPutExcelTombstone",
                                new RouteValueDictionary
                                    {
                                        {"controller", "Tombstone"},
                                        {"action", "OutPutExcelTombstone"}
                                    },
                                null,
                                null,
                                new MvcRouteHandler())
                        }
                          //墓碑落葬
                         ,new RouteDescriptor
                        {
                            Priority = 20,
                            Route = new Route(
                                "BuryPeopleTombstone",
                                new RouteValueDictionary
                                    {
                                        {"controller", "Tombstone"},
                                        {"action", "BuryPeopleTombstone"}
                                    },
                                null,
                                null,
                                new MvcRouteHandler())
                        }  
                        //墓碑解除落葬
                         ,new RouteDescriptor
                        {
                            Priority = 20,
                            Route = new Route(
                                "UnBuryPeopleTombstone",
                                new RouteValueDictionary
                                    {
                                        {"controller", "Tombstone"},
                                        {"action", "UnBuryPeopleTombstone"}
                                    },
                                null,
                                null,
                                new MvcRouteHandler())
                        }
                        
                        
                };
        }
    }
}