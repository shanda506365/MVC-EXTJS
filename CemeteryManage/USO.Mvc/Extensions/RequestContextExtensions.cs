
namespace USO.Mvc.Extensions
{
    using System.Web.Mvc;
    using System.Web.Routing;
    using USO.Domain;

    public static class RequestContextExtensions
    {
        public static UrlHelper UrlHelper(this RequestContext instance)
        {
            

            return new UrlHelper(instance);
        }
    }
}
