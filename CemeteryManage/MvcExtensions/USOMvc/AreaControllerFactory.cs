
namespace USO.Mvc
{
    using System;
    using System.Collections.Generic;
    using System.Web.Mvc;
    using MvcExtensions;
    using System.Web.Routing;

    /// <summary>
    /// 
    /// </summary>
    public class AreaControllerFactory : DefaultControllerFactory
    {

        /// <summary>
        /// Gets the container.
        /// </summary>
        /// <value>The container.</value>
        protected ContainerAdapter Container { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="container"></param>
        public AreaControllerFactory(ContainerAdapter container)
        {
            this.Container = container;
        }

        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="requestContext"></param>
        ///// <param name="controllerName"></param>
        ///// <returns></returns>
        //public override IController CreateController(RequestContext requestContext, string controllerName)
        //{
        //    string areaName = GetAreaName(requestContext.RouteData);
        //    if (!string.IsNullOrWhiteSpace(areaName))
        //    {
        //        string key = (areaName + "/" + controllerName).ToLowerInvariant();
        //        Controller service = this.Container.GetService<Controller>(key);
        //        if (service != null)
        //        {
        //            Type actionInvokerType;
        //            var actionInvokerRegistry = this.Container.GetService<TypeMappingRegistry<Controller, IActionInvoker>>();
        //            if (actionInvokerRegistry.IsRegistered(service.GetType()))
        //            {
        //                actionInvokerType = actionInvokerRegistry.Matching(service.GetType());
        //            }
        //            else
        //            {
        //                actionInvokerType = service is IAsyncController ?
        //                                    KnownTypes.AsyncActionInvokerType :
        //                                    KnownTypes.SyncActionInvokerType;
        //            }

        //            service.ActionInvoker = (IActionInvoker)this.Container.GetService(actionInvokerType);
        //            return service;
        //        }
        //    }
        //    return base.CreateController(requestContext, controllerName);
        //}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="route"></param>
        /// <returns></returns>
        public static string GetAreaName(RouteBase route)
        {
            IRouteWithArea area = route as IRouteWithArea;
            if (area != null)
            {
                return area.Area;
            }
            Route route2 = route as Route;
            if ((route2 != null) && (route2.DataTokens != null))
            {
                return (route2.DataTokens["area"] as string);
            }
            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="routeData"></param>
        /// <returns></returns>
        public static string GetAreaName(RouteData routeData)
        {
            object obj2;
            if (routeData.DataTokens.TryGetValue("area", out obj2))
            {
                return (obj2 as string);
            }
            return GetAreaName(routeData.Route);
        }

        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="requestContext"></param>
        ///// <param name="controllerType"></param>
        ///// <returns></returns>
        //protected override IController GetControllerInstance(RequestContext requestContext, Type controllerType)
        //{
        //    string name = controllerType.Name;
        //    if (name.EndsWith("Controller"))
        //    {
        //        name = name.Substring(0, name.Length - "Controller".Length);
        //    }
        //    string key = (controllerType.Assembly.GetName().Name + "/" + name).ToLowerInvariant();
        //    IController service = this.Container.GetService<Controller>(key);
        //    if (service != null)
        //    {
        //        return service;
        //    }
        //    return null;
        //}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="requestContext"></param>
        /// <param name="controllerName"></param>
        /// <returns></returns>
        protected override Type GetControllerType(RequestContext requestContext, string controllerName)
        {
            string areaName = GetAreaName(requestContext.RouteData);
            if (!string.IsNullOrWhiteSpace(areaName))
            {
                string key = (areaName + "/" + controllerName).ToLowerInvariant();
                Controller service = this.Container.GetService<Controller>(key);
                if (service != null)
                {
                    return service.GetType();
                }
            }
            return null;
        }

    }
}
