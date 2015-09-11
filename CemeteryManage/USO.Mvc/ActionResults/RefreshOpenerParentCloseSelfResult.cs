
namespace USO.Mvc.ActionResults
{
    using System;
    using System.Web.Mvc;

    public class RefreshOpenerParentCloseSelfResult : ActionResult
    {
        public override void ExecuteResult(ControllerContext context)
        {
            string script = "<script>if(window.opener){window.opener.parent.document.location = window.opener.parent.document.location;}";
            script += "window.open('', '_parent', '');";
            script += "window.close();</script>";

            context.RequestContext.HttpContext.Response.Write(script);
        }
    }
}
