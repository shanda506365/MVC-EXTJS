namespace USO.Mvc
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Web.Mvc;
    using System.Web.Mvc.Html;
    using System.Web.Routing;
    using USO.Dto;
    using USO.Domain;

    public class PagerHtmlHelper
    {
        private readonly HtmlHelper htmlHelper;

        public PagerHtmlHelper(HtmlHelper htmlHelper)
        {
            Check.Argument.IsNotNull(htmlHelper, "htmlHelper");

            this.htmlHelper = htmlHelper;
        }

        public string PagerUrl(int page, string pageParamName, UrlHelper url)
        {
            IDictionary<string, object> values = new Dictionary<string, object>();
            foreach (var key in htmlHelper.ViewContext.HttpContext.Request.QueryString.AllKeys)
            {
                if (!values.Keys.Contains(key))
                {
                    values.Add(key, htmlHelper.ViewContext.HttpContext.Request.QueryString[key]);
                }
            }

            foreach (KeyValuePair<string, object> pair in htmlHelper.ViewContext.RouteData.Values)
            {
                if (!values.Keys.Contains(pair.Key))
                {
                    values.Add(pair.Key, pair.Value);
                }
            }


            RouteValueDictionary newValues = new RouteValueDictionary();
            foreach (KeyValuePair<string, object> pair in values)
            {
                if (!pair.Key.Equals("controller", StringComparison.OrdinalIgnoreCase) &&
                    !pair.Key.Equals("action", StringComparison.OrdinalIgnoreCase))
                {
                    newValues[pair.Key] = pair.Value;
                }
            }

            if (page > 0)
            {
                newValues[pageParamName] = page;
            }

            var actionName = values["action"].ToString();
            var controllerName = values["controller"].ToString();

            return url.Action(actionName, controllerName, newValues);
        }
    }
}