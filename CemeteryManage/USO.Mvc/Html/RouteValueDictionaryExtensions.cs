
namespace USO.Mvc.Html
{
    using System.Diagnostics;
    using System.Web.Routing;
    using USO.Domain;

    public static class RouteValueDictionaryExtensions
    {
        [DebuggerStepThrough]
        public static RouteValueDictionary Copy(this RouteValueDictionary instance)
        {
         

            RouteValueDictionary routeValue = new RouteValueDictionary();
            foreach (var key in instance.Keys)
            {
                routeValue.Add(key, instance[key]);
            }

            return routeValue;
        }

        public static RouteValueDictionary Append(this RouteValueDictionary instance, string key, object value)
        {
          

            instance.Add(key, value);

            return instance;
        }
    }
}
