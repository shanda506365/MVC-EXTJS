
namespace USO.Mvc.ActionResults
{
    using System.Web.Mvc;

    public class RefreshSelfResult : ActionResult
    {
        private string _appendScripts = string.Empty;
        public override void ExecuteResult(ControllerContext context)
        {
            string script = "<script>" + _appendScripts;
            script += "location.href = location.href;";
            script += "</script>";

            context.RequestContext.HttpContext.Response.Write(script);
        }
        public RefreshSelfResult()
        {
            
        }
        public RefreshSelfResult(string message)
        {
            if (!string.IsNullOrEmpty(message))
            {
                _appendScripts = string.Format("alert('{0}');", message);
            }
        }
    }

    public class AlertResult:ActionResult
    {
        public AlertResult(string message)
        {
            _message = message;
        }
        private readonly string _message;
        public override void ExecuteResult(ControllerContext context)
        {
            string script = string.Format("<script>alert('{0}');history.back(-1);</script>", _message);
            context.RequestContext.HttpContext.Response.Write(script);
        }
    }
}
