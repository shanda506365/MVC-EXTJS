using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace USO.Mvc.ActionResults
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
    public class MyAuthorizeAttribute : FilterAttribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationContext filterContext)
        {

            string sessiontoken = filterContext.HttpContext.Request["login_sessiontoken"];
          
            var serviceSessiontoken = filterContext.HttpContext.Session["sessiontoken"];
            string serviceSessiontoken1 = string.Empty;
            if (serviceSessiontoken != null)
            {
                serviceSessiontoken1 = serviceSessiontoken.ToString();
            }
            //When user has not login yet
            if (serviceSessiontoken == null || sessiontoken != serviceSessiontoken1)
            {
                var redirectUrl = "/UserLogin" + "?RedirectPath=" + filterContext.HttpContext.Request.Url;
                if (!filterContext.HttpContext.Request.IsAjaxRequest())
                {
                    filterContext.Result = new RedirectResult(redirectUrl);
                }
                else
                {
                    filterContext.Result = new JsonResult
                    {
                        Data = new
                        {
                            ResultOutDto = "null",
                            success = false,
                            msg = "",
                            code = 0x0F000E33,
                            Redirect = redirectUrl
                        }
                    };
                }
            }
            else//判断权限
            {

            }


            return;
        }

    }
}
