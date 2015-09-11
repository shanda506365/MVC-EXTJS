namespace USO.Mvc.Providers
{
    using System.Web;
    using System.Web.Mvc;
    using USO.Infrastructure;
    using USO.Mvc.Filters;

    public class SingleLoginFilter : GlobalFilterProvider, IAuthorizationFilter
    {
       
        private readonly WorkContext _workContext;

        public SingleLoginFilter(WorkContext workContext)
        {
            _workContext = workContext;
        }
        
        public void OnAuthorization(AuthorizationContext filterContext)
        {
            ////是否允许单个登录
            //if (_settings == null || _settings.SingleLogin == false)
            //    return;

            ////只检查登录的用户，匿名用户不检查
            //if (_workContext == null || _workContext.CurrentUser == null)
            //    return;

            //var currentSessionID = string.Empty;
            //if (HttpContext.Current != null && HttpContext.Current.Session != null)
            //{
            //    //当前用户会话ID
            //    currentSessionID=HttpContext.Current.Session.SessionID;
            //}

            //var userSessionID = _workContext.CurrentUser.SessionId;

            ////如果不是相同的会话了，则强制用户重新登录
            //if (userSessionID != currentSessionID)
            //{
            //    _authenticationService.SignOut();

            //    filterContext.Result = new HttpUnauthorizedResult();//返回未授权Result

            //    //filterContext.Result = new RedirectToRouteResult
            //    //  (
            //    //    //"LogOn",
            //    //    new RouteValueDictionary
            //    //    (
            //    //      new
            //    //      {
            //    //          area = "USO.HomePage",
            //    //          controller = "Home",
            //    //          action = "Login"//,
            //    //          //redirect = filterContext..GetCurrentUrl()
            //    //      }
            //    //    )
            //    //  );
            //}
        }      

    }
}
