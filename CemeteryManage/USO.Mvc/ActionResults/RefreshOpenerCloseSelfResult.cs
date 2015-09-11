
namespace USO.Mvc.ActionResults
{
    using System;
    using System.Collections.Generic;
    using System.Web.Mvc;

    public class RefreshOpenerCloseSelfResult : ActionResult
    {
        public RefreshOpenerCloseSelfResult()
        {
            
        }

        public RefreshOpenerCloseSelfResult(string alertMsg)
        {
            AlertMsg = alertMsg;
        }

        public string AlertMsg { get; set; }


        public override void ExecuteResult(ControllerContext context)
        {
            var script = "<script>";
            if(!string.IsNullOrWhiteSpace(AlertMsg))
            {
                script += "alert('"+AlertMsg+"');";
            }
            script += "if(window.opener){window.opener.document.location = window.opener.document.location;}";
            script += "window.open('', '_parent', '');";
            script += "window.close();</script>";

            context.RequestContext.HttpContext.Response.Write(script);
        }
    }
}
