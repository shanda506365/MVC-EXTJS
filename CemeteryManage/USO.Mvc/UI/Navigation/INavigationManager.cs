
namespace USO.UI.Navigation
{
    using System.Collections.Generic;
    using System.Web.Routing;
    using USO.Core;

    public interface INavigationManager : IDependency
    {
        string GetUrl(string menuItemUrl, RouteValueDictionary routeValueDictionary);
    }
}