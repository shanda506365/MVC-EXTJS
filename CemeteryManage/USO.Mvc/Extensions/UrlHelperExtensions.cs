﻿
namespace USO.Mvc.Extensions
{
    using System;
    using System.Web.Mvc;
    using USO.Domain;
    using USO.Domain.Extensions;

    public static class UrlHelperExtensions
    {
        public static string AbsoluteAction(this UrlHelper urlHelper, Func<string> urlAction)
        {
            return urlHelper.MakeAbsolute(urlAction());
        }

        public static string AbsoluteAction(this UrlHelper urlHelper, string actionName)
        {
            return urlHelper.MakeAbsolute(urlHelper.Action(actionName));
        }

        public static string AbsoluteAction(this UrlHelper urlHelper, string actionName, object routeValues)
        {
            return urlHelper.MakeAbsolute(urlHelper.Action(actionName, routeValues));
        }

        public static string AbsoluteAction(this UrlHelper urlHelper, string actionName, string controller)
        {
            return urlHelper.MakeAbsolute(urlHelper.Action(actionName, controller));
        }

        public static string AbsoluteAction(this UrlHelper urlHelper, string actionName, string controller, object routeValues)
        {
            return urlHelper.MakeAbsolute(urlHelper.Action(actionName, controller, routeValues));
        }

        private static string MakeAbsolute(this UrlHelper urlHelper, string url)
        {
            var siteUrl = urlHelper.RequestContext.HttpContext.Request.ToRootUrlString();
            return siteUrl + url;
        }

        public static string OpenIdIcon(this UrlHelper instance, string icon)
        {
            Check.Argument.IsNotNull(instance, "helper");
            Check.Argument.IsNotNullOrEmpty(icon, "icon");

            return instance.Content("~/Content/images/openid/{0}.png".FormatWith(icon));
        }

        public static string InputValidationErrorIcon(this UrlHelper instance)
        {
            Check.Argument.IsNotNull(instance, "helper");

            return instance.Content("~/Content/images/forms/exclamation.png");
        }
       
    }
}
