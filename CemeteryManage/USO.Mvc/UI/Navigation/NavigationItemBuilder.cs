
namespace USO.UI.Navigation
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Routing;

    public class NavigationItemBuilder : NavigationBuilder
    {

      

        public NavigationItemBuilder Caption(string caption)
        {
            return this;
        }

        public NavigationItemBuilder Position(string position)
        {
            return this;
        }

        public NavigationItemBuilder Url(string url)
        {
            return this;
        }

      


        public NavigationItemBuilder Action(RouteValueDictionary values)
        {
            return values != null
                       ? Action(values["action"] as string, values["controller"] as string, values)
                       : Action(null, null, new RouteValueDictionary());
        }

        public NavigationItemBuilder Action(string actionName)
        {
            return Action(actionName, null, new RouteValueDictionary());
        }

        public NavigationItemBuilder Action(string actionName, string controllerName)
        {
            return Action(actionName, controllerName, new RouteValueDictionary());
        }

        public NavigationItemBuilder Action(string actionName, string controllerName, object values)
        {
            return Action(actionName, controllerName, new RouteValueDictionary(values));
        }

      
    }
}