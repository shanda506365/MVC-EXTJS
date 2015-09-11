using System.Net;
using Elmah;
using USO.Domain;

namespace USO.Store
{
    using System;
    using System.Web;
    using System.Web.Mvc;
    using MvcExtensions;
    using MvcExtensions.Unity;
    using USO.Core;
    using USO.Core.Tasks;
    using USO.Mvc;
    using USO.Mvc.BootstrapperTasks;
    using USO.Mvc.Filters;
    using USO.Mvc.ModelBinder;

    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : UnityMvcApplication
    {
        public MvcApplication()
        {
            try
            {
                Bootstrapper.BootstrapperTasks
                            .Include<RegisterModelMetadata>()
                            .Include<RegisterGlobalFilterProviders>()
                            .Include<RegisterDependencies>()
                            .Include<ConfigureRoutes>();
            }
            catch (Exception ex)
            {
                throw ex;
            }

            //TODO:个性化设置
            System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("zh-CN");
        }
        protected override void OnStart()
        {
            base.OnStart();
            Adapter.GetServices<IBot>();//开始执行后台任务
        }

        public void ErrorLog_Filtering(object sender, ExceptionFilterEventArgs e)
        {
            Check.Argument.IsNotNull(e, "e");
            var exception = e.Exception.GetBaseException() as HttpException;
            if ((exception != null) && (exception.GetHttpCode() == (int)HttpStatusCode.NotFound))
            {
                e.Dismiss();
            }
        }
        protected void Application_Error(object sender, EventArgs e)
        {
            //获取Exception
            Exception ex = this.Context.Server.GetLastError();
            //处理Exception
            if (ex != null)
            {
                Response.Cookies["errorInfo"].Value = ex.Message.ToString();
            }
            //清除当前的输出
            //this.Context.Response.Clear();
            //转向执行你希望展示给用户看的错误提示页面样子（此时网址依然是出错的那个页面，但是展示的内容就完全是你自己指定的页面了）
            //this.Context.Server.Transfer("/Error?source=code&msg=" + ex.Message);
        }
    }
}