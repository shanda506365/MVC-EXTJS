
namespace USO.Mvc.ActionResults
{
    using System.Web.Mvc;

    public class RefreshParentResult : ActionResult
    {
        private readonly string _queryString;

        public RefreshParentResult()
        {
            
        }

        public RefreshParentResult(string queryString)
        {
            if (queryString.StartsWith("&") || queryString.StartsWith("?"))
                _queryString = queryString.Substring(1);
            else
                _queryString = queryString;
        }

        public override void ExecuteResult(ControllerContext context)
        {
            if (string.IsNullOrWhiteSpace(_queryString))
            {
                var script = "<script>window.parent.document.location = window.parent.document.location;</script>";
                context.RequestContext.HttpContext.Response.Write(script);
            }
            else
            {
                var script = @"<script>
                                    var url = window.parent.document.location.href;
                                    if(url.indexOf('?') >= 0)
                                    {
                                        url+='&';
                                    }
                                    else
                                    {
                                        url+='?';
                                    }
                                    url += '"+_queryString+ @"';
                                    window.parent.document.location.href = url;
                               </script>";

                context.RequestContext.HttpContext.Response.Write(script);
            }
        }
    }
}
