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
    using System.Web.Routing;
    using System.Text;

    public static class CheckListExtensions
    {
        public static MvcHtmlString CheckBoxList(this HtmlHelper htmlHelper, String name, IEnumerable<SelectListItem> selectList, IDictionary<String, Object> htmlAttributes) {   
  
          TagBuilder list = new TagBuilder("div");   
          //list.MergeAttributes<String, Object>(htmlAttributes);   
  
          StringBuilder items = new StringBuilder();   
          Int32 index = 1;   
          foreach (SelectListItem i in selectList) {   
  
            TagBuilder input = new TagBuilder("input");
            input.MergeAttributes<String, Object>(htmlAttributes);   

            if (i.Selected) input.MergeAttribute("checked", "checked");   
            input.MergeAttribute("id", String.Concat(name, index));   
            input.MergeAttribute("name", name);   
            input.MergeAttribute("type", "checkbox");   
            input.MergeAttribute("value", i.Value);   
  
            TagBuilder label = new TagBuilder("label");   
            label.MergeAttribute("for", String.Concat(name, index));   
            label.InnerHtml = i.Text;

            items.AppendFormat("{0}{1}&nbsp;&nbsp;&nbsp;&nbsp;", input.ToString(TagRenderMode.Normal), label.ToString(TagRenderMode.Normal));   
  
            index++;   
  
          }   
  
          list.InnerHtml = items.ToString();   
  
          return MvcHtmlString.Create(list.ToString(TagRenderMode.Normal));   
  
        }

        public static MvcHtmlString CheckBoxListFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, IEnumerable<SelectListItem> selectList)
        {
            return CheckBoxListFor(htmlHelper, expression, selectList, (IDictionary<string, object>)null);
        }

        public static MvcHtmlString CheckBoxListFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, IEnumerable<SelectListItem> selectList, object htmlAttributes)
        {
            return CheckBoxListFor(htmlHelper, expression, selectList, HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes));
        }


        public static MvcHtmlString CheckBoxListFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, IEnumerable<SelectListItem> selectList, IDictionary<String, Object> htmlAttributes)
        {

            ModelMetadata metadata = ModelMetadata.FromLambdaExpression(expression, htmlHelper.ViewData);

            String field = ExpressionHelper.GetExpressionText(expression);

            String name = htmlHelper.ViewContext.ViewData.TemplateInfo.GetFullHtmlFieldId(field);

            if (String.IsNullOrEmpty(name))
                throw new ArgumentException("Name is required", "name");

            selectList = GetSelectList(htmlHelper, name, selectList, true);

            return CheckBoxList(htmlHelper, metadata.DisplayName ?? metadata.PropertyName ?? field, selectList, htmlAttributes == null ? new RouteValueDictionary() : new RouteValueDictionary(htmlAttributes));

        }  

        private static IEnumerable<SelectListItem> GetSelectData(this HtmlHelper htmlHelper, String name)
        {

            Object data = null;

            if (htmlHelper.ViewData != null)
                data = htmlHelper.ViewData.Eval(name);

            if (data == null)
                throw new InvalidOperationException(String.Format("There is no ViewData item of type '{0}' that has the key '{1}'.", "IEnumerable<SelectListItem>", name));

            IEnumerable<SelectListItem> select = data as IEnumerable<SelectListItem>;
            if (select == null)
                throw new InvalidOperationException(String.Format("The ViewData item that has the key '{0}' is of type '{1}' but must be of type '{2}'.", name, data.GetType().FullName, "IEnumerable<SelectListItem>"));

            return select;

        } // GetSelectData   

        // GetSelectList   
        private static IEnumerable<SelectListItem> GetSelectList(this HtmlHelper htmlHelper, String name, IEnumerable<SelectListItem> selectList, Boolean allowMultiple)
        {

            Boolean usedViewData = false;

            if (selectList == null)
            {
                selectList = htmlHelper.GetSelectData(name);
                usedViewData = true;
            }

            Object defaultValue = null;
            ModelState modelState;
            if (htmlHelper.ViewData.ModelState.TryGetValue(name, out modelState))
            {
                if (modelState.Value != null)
                {
                    defaultValue = modelState.Value.ConvertTo(allowMultiple ? typeof(String[]) : typeof(String), null);
                }
            }

            if (!usedViewData)
            {
                if (defaultValue == null)
                    defaultValue = htmlHelper.ViewData.Eval(name);
            }

            if (defaultValue != null)
            {

                IEnumerable defaultValues = (allowMultiple) ? defaultValue as IEnumerable : new[] { defaultValue };

                IEnumerable<String> values = from Object value in defaultValues select Convert.ToString(value, CultureInfo.CurrentCulture);

                HashSet<String> selectedValues = new HashSet<string>(values, StringComparer.OrdinalIgnoreCase);

                List<SelectListItem> newSelectList = new List<SelectListItem>();
                foreach (SelectListItem item in selectList)
                {
                    item.Selected = (item.Value != null) ? selectedValues.Contains(item.Value) : selectedValues.Contains(item.Text);
                    newSelectList.Add(item);
                }
                selectList = newSelectList;

            }

            return selectList;

        }
    }
}
