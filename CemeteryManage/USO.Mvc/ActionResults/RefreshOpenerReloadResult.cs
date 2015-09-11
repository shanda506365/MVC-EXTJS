
namespace USO.Mvc.ActionResults
{
    using System.Web.Mvc;
    using USO.Domain.Extensions;

    public class RefreshOpenerReloadResult : ActionResult
    {
        public RefreshOpenerReloadResult(string reloadUrl)
        {
            ReloadUrl = reloadUrl;
        }

        public string ReloadUrl { get; set; }

        public override void ExecuteResult(ControllerContext context)
        {
            string script = "<script>if(window.opener){window.opener.document.location = window.opener.document.location;}";
            script += "window.open('', '_parent', '');";
            script += "location.href ='{0}'".FormatWith(ReloadUrl);
            script += "</script>";

            context.RequestContext.HttpContext.Response.Write(script);
        }
    }
}
