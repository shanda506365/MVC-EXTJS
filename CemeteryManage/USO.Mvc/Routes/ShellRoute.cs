
namespace USO.Mvc.Routes
{
    using System.Web;
    using System.Web.Mvc;
    using System.Web.Routing;

    public class ShellRoute : RouteBase, IRouteWithArea
    {
        private readonly RouteBase _route;
        public ShellRoute(RouteBase route)
        {
            _route = route;

            var routeWithArea = route as IRouteWithArea;
            if (routeWithArea != null)
            {
                Area = routeWithArea.Area;
            }

            var routeWithDataTokens = route as Route;
            if ((routeWithDataTokens != null) && (routeWithDataTokens.DataTokens != null))
            {
                Area = (routeWithDataTokens.DataTokens["area"] as string);
            }
        }

        public string Area { get; private set; }

        public override RouteData GetRouteData(HttpContextBase httpContext)
        {
            var routeData = _route.GetRouteData(httpContext);
            if (routeData == null)
                return null;

            // otherwise wrap handler and return it
            //routeData.RouteHandler = new RouteHandler(_workContextAccessor, routeData.RouteHandler);
            //routeData.DataTokens["IWorkContextAccessor"] = _workContextAccessor;

            return routeData;
        }


        public override VirtualPathData GetVirtualPath(RequestContext requestContext, RouteValueDictionary values)
        {
            var virtualPath = _route.GetVirtualPath(requestContext, values);
            if (virtualPath == null)
                return null;

            return virtualPath;
        }      
    }
}
