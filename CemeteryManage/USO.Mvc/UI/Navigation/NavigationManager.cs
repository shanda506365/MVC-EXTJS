
namespace USO.UI.Navigation
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;
    using System.Web.Routing;
    using MvcExtensions;
    using USO.Infrastructure;

    public class NavigationManager : INavigationManager
    {
        private readonly IEnumerable<INavigationProvider> navigationProviders;
        private readonly UrlHelper urlHelper;
        readonly WorkContext workContext;


        public NavigationManager(
            ContainerAdapter container,
            UrlHelper urlHelper, 
            WorkContext workContext
            )
        {
            this.navigationProviders = container.GetServices<INavigationProvider>();
            this.urlHelper = urlHelper;
            this.workContext = workContext;
        }

       

      

        public string GetUrl(string menuItemUrl, RouteValueDictionary routeValueDictionary)
        {
            var url = string.IsNullOrEmpty(menuItemUrl) && (routeValueDictionary == null || routeValueDictionary.Count == 0)
              ? "~/"
              : !string.IsNullOrEmpty(menuItemUrl)
                    ? menuItemUrl
                    : urlHelper.RouteUrl(routeValueDictionary);

            if (!string.IsNullOrEmpty(url) && urlHelper.RequestContext.HttpContext != null &&
                !(url.StartsWith("http://") || url.StartsWith("https://") || url.StartsWith("/")))
            {
                if (url.StartsWith("~/"))
                {
                    url = url.Substring(2);
                }
                var appPath = urlHelper.RequestContext.HttpContext.Request.ApplicationPath;
                if (appPath == "/")
                    appPath = "";
                url = string.Format("{0}/{1}", appPath, url);
            }
            return url;
        }


        

        private static string SelectBestPositionValue(IEnumerable<string> positions)
        {
            var comparer = new FlatPositionComparer();
            return positions.Aggregate(string.Empty,
                                       (agg, pos) =>
                                       string.IsNullOrEmpty(agg)
                                           ? pos
                                           : string.IsNullOrEmpty(pos)
                                                 ? agg
                                                 : comparer.Compare(agg, pos) < 0 ? agg : pos);
        }
    }
}