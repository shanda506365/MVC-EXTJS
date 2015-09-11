
namespace USO.Mvc.ActionResults
{
    using System.Web.Mvc;


    public class RefreshOpenerReloadSelfResult : ActionResult
    {
        public override void ExecuteResult(ControllerContext context)
        {
            string script = "<script>window.opener.document.location = window.opener.document.location;";
            script += "location.href = location.href;";
            script += "</script>";

            context.RequestContext.HttpContext.Response.Write(script);
        }
    }

    public class RefreshOpenerRedirectResult : ActionResult
    {
        public string RedirectUrl { get; set; }

        public RefreshOpenerRedirectResult(string redirectUrl)
        {
            RedirectUrl = redirectUrl;
        }

        public override void ExecuteResult(ControllerContext context)
        {
            string script = "<script>window.opener.document.location = window.opener.document.location;";
            script += "location.href = '" + RedirectUrl + "';";
            script += "</script>";

            context.RequestContext.HttpContext.Response.Write(script);
        }
    }
}
