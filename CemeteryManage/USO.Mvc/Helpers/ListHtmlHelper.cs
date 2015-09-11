namespace USO.Mvc
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Web.Mvc;
    using System.Web.Mvc.Html;
    using System.Web.Routing;
    using USO.Dto;
    using USO.Mvc.ViewModels;
    using System.Globalization;
    using USO.Domain;
    using USO.Domain.Extensions;
    using System.Web;

    public class ListHtmlHelper
    {
        private readonly HtmlHelper htmlHelper;

        public ListHtmlHelper(HtmlHelper htmlHelper)
        {
            Check.Argument.IsNotNull(htmlHelper, "htmlHelper");

            this.htmlHelper = htmlHelper;
        }

        public IHtmlString Pager<TItem>() where TItem : class
        {
            PagedListViewModel<TItem> model = (PagedListViewModel<TItem>)htmlHelper.ViewContext.ViewData.Model;

            return Pager(null, null, null, htmlHelper.ViewContext.RouteData.Values, "page", model.PageCount, model.ItemPerPage, 2, model.CurrentPage);
        }

        public IHtmlString Pager<TItem>(PagedListViewModel<TItem> model) where TItem : class
        {
            return Pager(model, model.ItemPerPage);
        }

        public IHtmlString Pager<TItem>(PagedListViewModel<TItem> model, int noOfPageToShow) where TItem : class
        {
            IDictionary<string, object> values = new Dictionary<string, object>();
            foreach (var key in htmlHelper.ViewContext.HttpContext.Request.QueryString.AllKeys)
            {
                if (!values.Keys.Contains(key))
                {
                    values.Add(key, htmlHelper.ViewContext.HttpContext.Request.QueryString[key]);
                }
            }

            foreach (KeyValuePair<string, object> pair in htmlHelper.ViewContext.RouteData.Values)
            {
                if (!values.Keys.Contains(pair.Key))
                {
                    values.Add(pair.Key, pair.Value);
                }
            }

            return Pager(null, null, null, values, "page", model.PageCount, noOfPageToShow, 1, model.CurrentPage);
        }

        private IHtmlString Pager(string routeName, string actionName, string controllerName, IDictionary<string, object> values, string pageParamName, int pageCount, int noOfPageToShow, int noOfPageInEdge, int currentPage)
        {
            Func<string, int, string, string> getPageLink = (text, page, className) =>
                                                                    {
                                                                        RouteValueDictionary newValues = new RouteValueDictionary();

                                                                        foreach (KeyValuePair<string, object> pair in values)
                                                                        {
                                                                            if (!pair.Key.Equals("controller", StringComparison.OrdinalIgnoreCase) &&
                                                                                !pair.Key.Equals("action", StringComparison.OrdinalIgnoreCase))
                                                                            {
                                                                                newValues[pair.Key] = pair.Value;
                                                                            }
                                                                        }

                                                                        if (page > 0)
                                                                        {
                                                                            newValues[pageParamName] = page;
                                                                        }

                                                                        string link;

                                                                        if (!string.IsNullOrWhiteSpace(routeName))
                                                                        {
                                                                            link = htmlHelper.RouteLink(text, routeName, newValues).ToHtmlString();
                                                                        }
                                                                        else
                                                                        {
                                                                            actionName = actionName ?? values["action"].ToString();
                                                                            controllerName = controllerName ?? values["controller"].ToString();

                                                                            IDictionary<string, object> htmlAttrtibutes = new Dictionary<string, object>();
                                                                            htmlAttrtibutes.Add("class", className);

                                                                            if (string.IsNullOrWhiteSpace(className))
                                                                                link = htmlHelper.ActionLink(text, actionName, controllerName, newValues, null).ToHtmlString();
                                                                            else
                                                                                link = htmlHelper.ActionLink(text, actionName, controllerName, newValues, htmlAttrtibutes).ToHtmlString();
                                                                        }

                                                                        return string.Concat(" ", link);
                                                                    };

            StringBuilder pagerHtml = new StringBuilder();

            if (pageCount > 1)
            {
                pagerHtml.Append("<div class=\"page-navigation Noprint\">");

                double half = Math.Ceiling(Convert.ToDouble(Convert.ToDouble(noOfPageToShow) / 2));

                int start = Convert.ToInt32((currentPage > half) ? Math.Max(Math.Min((currentPage - half), (pageCount - noOfPageToShow)), 0) : 0);
                int end = Convert.ToInt32((currentPage > half) ? Math.Min(currentPage + half, pageCount) : Math.Min(noOfPageToShow, pageCount));

                pagerHtml.Append(currentPage > 1 ? getPageLink("上一页", currentPage - 1, "pre") : " <span class=\"pre\">上一页</span>");

                if (start > 0)
                {
                    int startingEnd = Math.Min(noOfPageInEdge, start);

                    for (int i = 0; i < startingEnd; i++)
                    {
                        int pageNo = i + 1;

                        pagerHtml.Append(getPageLink(pageNo.ToString(CultureInfo.CurrentCulture), pageNo, string.Empty));
                    }

                    if (noOfPageInEdge < start)
                    {
                        pagerHtml.Append(@"<span class=""etc"">...</span>");
                    }
                }

                for (int i = start; i < end; i++)
                {
                    int pageNo = i + 1;

                    pagerHtml.Append(pageNo == currentPage ? " <a class=\"now\">{0}</a>".FormatWith(pageNo) : getPageLink(pageNo.ToString(CultureInfo.CurrentCulture), pageNo, string.Empty));
                }

                if (end < pageCount)
                {
                    if ((pageCount - noOfPageInEdge) > end)
                    {
                        pagerHtml.Append(@"<span class=""etc"">...</span>");
                    }

                    int endingStart = Math.Max(pageCount - noOfPageInEdge, end);

                    for (int i = endingStart; i < pageCount; i++)
                    {
                        int pageNo = i + 1;
                        pagerHtml.Append(getPageLink(pageNo.ToString(CultureInfo.CurrentCulture), pageNo, string.Empty));
                    }
                }

                pagerHtml.Append(currentPage < pageCount ? getPageLink("下一页", currentPage + 1, "next") : " <span class=\"next\">下一页</span>");

                pagerHtml.Append(@"<span class=""go-page"">到第<input type=""textbox"" class=""pageText"" maxNum=""100"" value=""{0}"" />页 <input type=""button"" class=""view pagerbutton"" value=""确定"" /></span>".FormatWith(currentPage));

                pagerHtml.Append("</div>");
            }

            return new HtmlString(pagerHtml.ToString());
        }
    }
}