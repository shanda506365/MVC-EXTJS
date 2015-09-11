
namespace USO.Mvc.Html
{
    using System.Web.Mvc;
    using USO.UI.PageClass;
    using USO.UI.PageTitle;

    public static class LayoutExtensions
    {

        public static void AddTitleParts(this HtmlHelper html, params string[] titleParts)
        {
            DependencyResolver.Current.GetService<IPageTitleBuilder>().AddTitleParts(titleParts);
        }

        public static void AppendTitleParts(this HtmlHelper html, params string[] titleParts)
        {
            DependencyResolver.Current.GetService<IPageTitleBuilder>().AppendTitleParts(titleParts);
        }

        public static MvcHtmlString Title(this HtmlHelper html, params string[] titleParts)
        {
            IPageTitleBuilder pageTitleBuilder = DependencyResolver.Current.GetService<IPageTitleBuilder>();

            html.AppendTitleParts(titleParts);

            return MvcHtmlString.Create(html.Encode(pageTitleBuilder.GenerateTitle()));
        }

        public static MvcHtmlString TitleForPage(this HtmlHelper html, params string[] titleParts)
        {
            if (titleParts == null || titleParts.Length < 1)
                return null;

            html.AppendTitleParts(titleParts);

            return MvcHtmlString.Create(html.Encode(titleParts[0]));
        }

        public static void AddPageClassNames(this HtmlHelper html, params object[] classNames)
        {
            DependencyResolver.Current.GetService<IPageClassBuilder>().AddClassNames(classNames);
        }

        public static MvcHtmlString ClassForPage(this HtmlHelper html, params object[] classNames)
        {
            IPageClassBuilder pageClassBuilder = DependencyResolver.Current.GetService<IPageClassBuilder>();

            html.AddPageClassNames(classNames);
            //todo: (heskew) need ContentItem.ContentType
            html.AddPageClassNames(html.ViewContext.RouteData.Values["area"]);

            return MvcHtmlString.Create(html.Encode(pageClassBuilder.ToString()));
        }

    }
}