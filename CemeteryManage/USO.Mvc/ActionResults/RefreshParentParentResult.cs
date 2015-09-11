
namespace USO.Mvc.ActionResults
{
    using System;
    using System.Web.Mvc;


    public class RefreshParentParentResult : ActionResult
    {
        public override void ExecuteResult(ControllerContext context)
        {
            string script = "<script>window.parent.parent.document.location = window.parent.parent.document.location;</script>";

            context.RequestContext.HttpContext.Response.Write(script);
        }
    }
}
