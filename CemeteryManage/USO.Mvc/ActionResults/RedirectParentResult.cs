
namespace USO.Mvc.ActionResults
{
    using System;
    using System.Web.Mvc;

    public class RedirectParentResult : ActionResult
    {
        private string url;

        public RedirectParentResult(string url)
        {
            this.url = url;
        }

        public override void ExecuteResult(ControllerContext context)
        {
            string script = string.Format("<script>window.parent.document.location = {0}; </script>", ("\"" + url + "\"" ?? "window.parent.document.location"));

            context.RequestContext.HttpContext.Response.Write(script);
        }
    }
}
