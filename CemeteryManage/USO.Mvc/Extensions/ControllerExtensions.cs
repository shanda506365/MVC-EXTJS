
namespace USO.Mvc.Extensions
{
    using System.Collections.Generic;
    using System.Web.Mvc;
    using MvcExtensions;
    using USO.Domain;
    using USO.Dto;
    using USO.Mvc.Infrastructure;

    public static class ControllerExtensions
    {
        public static ActionResult AdaptivePostRedirectGet(this Controller instance, string url)
        {
            Check.Argument.IsNotNull(instance, "instance");
            Check.Argument.IsNotNullOrEmpty(url, "url");

            return new AdaptivePostRedirectGetResult(url, new[] { new CamelCasedJsonConverter() });
        }

        public static ActionResult AdaptivePostRedirectGet(this Controller instance, IEnumerable<RuleViolation> ruleViolations, object model, string url)
        {
            Check.Argument.IsNotNull(instance, "instance");
            Check.Argument.IsNotNullOrEmpty(url, "url");

            if (ruleViolations != null)
            {
                instance.ModelState.Merge(ruleViolations);
            }

            instance.ViewData.Model = model;

            return AdaptivePostRedirectGet(instance, url);
        }

        public static ActionResult AdaptiveView(this Controller instance, string viewName = null)
        {
            Check.Argument.IsNotNull(instance, "instance");

            var viewResult = new AdaptiveViewResult(new[] { new CamelCasedJsonConverter() }) { ViewData = instance.ViewData, TempData = instance.TempData };

            if (!string.IsNullOrWhiteSpace(viewName))
                viewResult.ViewName = viewName;

            return viewResult;
        }

        public static ActionResult AdaptiveView(this Controller instance, IEnumerable<RuleViolation> ruleViolations, object model, string viewName = null)
        {
            Check.Argument.IsNotNull(instance, "instance");

            if (ruleViolations != null)
            {
                instance.ModelState.Merge(ruleViolations);
            }

            instance.ViewData.Model = model;

            return AdaptiveView(instance, viewName);
        }
    }
}
