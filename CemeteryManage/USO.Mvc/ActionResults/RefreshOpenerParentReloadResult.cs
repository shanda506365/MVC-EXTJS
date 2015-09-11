
namespace USO.Mvc.ActionResults
{
    using System.Web.Mvc;
    using USO.Domain.Extensions;

    public class RefreshOpenerParentReloadResult : ActionResult
    {
        public RefreshOpenerParentReloadResult(string reloadUrl)
        {
            ReloadUrl = reloadUrl;
        }

        public string ReloadUrl { get; set; }

        public override void ExecuteResult(ControllerContext context)
        {
            string script = "<script>if(window.opener){window.opener.parent.document.location = window.opener.parent.document.location;}";
            script += "window.open('', '_parent', '');";
            script += "location.href ='{0}'".FormatWith(ReloadUrl);
            script += "</script>";

            context.RequestContext.HttpContext.Response.Write(script);
        }
    }
}
