namespace USO.Mvc.Html
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Web.Mvc;
    using System.Web.Mvc.Html;
    using System.Linq.Expressions;
    using USO.Dto;
    using USO.Domain;

    public static class RadioListExtensions
    {
        public static MvcHtmlString RadioButtonList(this HtmlHelper htmlHelper, string name)
        {
            return RadioButtonList(htmlHelper, name, (IDictionary<string, object>)null);
        }

        public static MvcHtmlString RadioButtonList(this HtmlHelper htmlHelper, string name, object htmlAttributes)
        {
            return RadioButtonList(htmlHelper, name, HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes));
        }

        public static MvcHtmlString RadioButtonList(this HtmlHelper htmlHelper, string name, IDictionary<string, object> htmlAttributes)
        {
            IEnumerable<SelectListItem> selectList = htmlHelper.GetSelectData(name);
            return htmlHelper.RadioButtonListInternal(name, selectList, true /* usedViewData */, htmlAttributes);
        }

        public static MvcHtmlString RadioButtonList(this HtmlHelper htmlHelper, string name, IEnumerable<SelectListItem> selectList)
        {
            return RadioButtonList(htmlHelper, name, selectList, (IDictionary<string, object>)null);
        }

        public static MvcHtmlString RadioButtonList(this HtmlHelper htmlHelper, string name, IEnumerable<SelectListItem> selectList, object htmlAttributes)
        {
            return RadioButtonList(htmlHelper, name, selectList, HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes));
        }

        public static MvcHtmlString RadioButtonList(this HtmlHelper htmlHelper, string name, IEnumerable<SelectListItem> selectList, IDictionary<string, object> htmlAttributes)
        {
            return htmlHelper.RadioButtonListInternal(name, selectList, false /* usedViewData */, htmlAttributes);
        }

        public static MvcHtmlString RadioButtonListFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, IEnumerable<SelectListItem> selectList) {
            return RadioButtonListFor(htmlHelper, expression, selectList, null /* htmlAttributes */);
        }

        public static MvcHtmlString RadioButtonListFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression,IEnumerable<SelectListItem> selectList, object htmlAttributes) {
            return RadioButtonListFor(htmlHelper, expression, selectList, HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes));
        }

        public static MvcHtmlString RadioButtonListFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, IEnumerable<SelectListItem> selectList, IDictionary<string, object> htmlAttributes)
        {
            ModelMetadata metadata = ModelMetadata.FromLambdaExpression(expression, htmlHelper.ViewData);

            String field = ExpressionHelper.GetExpressionText(expression);

            String name = htmlHelper.ViewContext.ViewData.TemplateInfo.GetFullHtmlFieldId(field);

            if (String.IsNullOrEmpty(name))
                throw new ArgumentException("Name is required", "name");

            return RadioButtonListInternal(htmlHelper,
                                     metadata.DisplayName ?? metadata.PropertyName ?? field,
                                     selectList,
                                     false,
                                     htmlAttributes);
        }

        private static IEnumerable<SelectListItem> GetSelectData(this HtmlHelper htmlHelper, string name)
        {
            object o = null;
            if (htmlHelper.ViewData != null)
            {
                o = htmlHelper.ViewData.Eval(name);
            }
            if (o == null)
            {
                throw new InvalidOperationException(
                    String.Format(
                        CultureInfo.CurrentCulture,
                        "Missing select data",
                        name,
                        typeof(IEnumerable<SelectListItem>)));
            }
            IEnumerable<SelectListItem> selectList = o as IEnumerable<SelectListItem>;
            if (selectList == null)
            {
                throw new InvalidOperationException(
                    String.Format(
                        CultureInfo.CurrentCulture,
                        "Wrong select data type",
                        name,
                        o.GetType().FullName,
                        typeof(IEnumerable<SelectListItem>)));
            }
            return selectList;
        }

        private static MvcHtmlString RadioButtonListInternal(this HtmlHelper htmlHelper, string name, IEnumerable<SelectListItem> selectList, bool usedViewData, IDictionary<string, object> htmlAttributes)
        {
            Check.Argument.IsNotNullOrEmpty(name, "name");

            if (selectList == null)
            {
                throw new ArgumentNullException("selectList");
            }

            // If we haven't already used ViewData to get the entire list of items then we need to
            // use the ViewData-supplied value before using the parameter-supplied value.
            if (!usedViewData)
            {
                object defaultValue = htmlHelper.ViewData.Eval(name);

                if (defaultValue != null)
                {
                    IEnumerable defaultValues = new[] { defaultValue };
                    IEnumerable<string> values = from object value in defaultValues select Convert.ToString(value, CultureInfo.CurrentCulture);
                    HashSet<string> selectedValues = new HashSet<string>(values, StringComparer.OrdinalIgnoreCase);
                    List<SelectListItem> newSelectList = new List<SelectListItem>();

                    foreach (SelectListItem item in selectList)
                    {
                        item.Selected = (item.Value != null) ? selectedValues.Contains(item.Value) : selectedValues.Contains(item.Text);
                        newSelectList.Add(item);
                    }

                    selectList = newSelectList;
                }
            }

            //IEnumerable<MvcHtmlString> radioButtons = selectList.Select<SelectListItem, MvcHtmlString>(item => htmlHelper.RadioButton(name, item.Value, item.Selected, htmlAttributes));

            //return radioButtons.ToArray();

            TagBuilder tableTag = new TagBuilder("table");
            tableTag.AddCssClass("radio-main");
            var trTag = new TagBuilder("tr");
            foreach (var item in selectList)
            {
                var tdTag = new TagBuilder("td");
                var rbValue = item.Value ?? item.Text;
                var rbId = name + "_" + rbValue;

                TagBuilder radioTag = new TagBuilder("input");
                radioTag.MergeAttributes<String, Object>(htmlAttributes);   

                if (item.Selected) radioTag.MergeAttribute("checked", "checked");
                radioTag.MergeAttribute("id", rbId);
                radioTag.MergeAttribute("name", name);
                radioTag.MergeAttribute("type", "radio");
                radioTag.MergeAttribute("value", rbValue);

                var labelTag = new TagBuilder("label");
                labelTag.MergeAttribute("for", rbId);
                labelTag.MergeAttribute("id", rbId + "_label");
                labelTag.InnerHtml = item.Text;

                tdTag.InnerHtml = radioTag.ToString() + labelTag.ToString();

                trTag.InnerHtml += tdTag.ToString();
            }
            tableTag.InnerHtml = trTag.ToString();
            return MvcHtmlString.Create(tableTag.ToString()); 
        }
    }
}
