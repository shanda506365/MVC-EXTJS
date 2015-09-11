namespace USO.Mvc.Razor
{
    using System.Web.Mvc;
    using USO.Infrastructure;


    /// <summary>
    /// This interface ensures all base view pages implement the 
    /// same set of additional members
    /// </summary>
    public interface IUSOViewPage
    {
        WorkContext WorkContext { get; }
    }

    public abstract class WebViewPage<TModel> : System.Web.Mvc.WebViewPage<TModel>, IUSOViewPage
    {
        private WorkContext _workContext;

        public WorkContext WorkContext { get { return _workContext; } }

        //public Settings Settings { get; set; }

        public override void InitHelpers()
        {
            base.InitHelpers();

            _workContext = DependencyResolver.Current.GetService<WorkContext>();
        }

     

        public bool HasText(object thing)
        {
            return !string.IsNullOrWhiteSpace(thing as string);
        }
    }

    public abstract class WebViewPage : WebViewPage<dynamic>
    {
    }
}
