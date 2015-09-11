
namespace USO.Mvc.Html
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Text;
    using System.Text.RegularExpressions;
    using System.Web;
    using System.Web.Mvc;
    using System.Web.Mvc.Html;
    using System.Web.Routing;
    using USO.Domain;
    using USO.Mvc.Extensions;
    using USO.Utility;

    public static class HtmlHelperExtensions
    {
        //public static string NameOf<T>(this HtmlHelper<T> html, Expression<Action<T>> expression)
        //{
        //    return Reflect.NameOf(html.ViewData.Model, expression);
        //}

        //public static string NameOf<T, TResult>(this HtmlHelper<T> html, Expression<Func<T, TResult>> expression)
        //{
        //    return Reflect.NameOf(html.ViewData.Model, expression);
        //}

        public static string FieldNameFor<T, TResult>(this HtmlHelper<T> html, Expression<Func<T, TResult>> expression)
        {
            return html.ViewData.TemplateInfo.GetFullHtmlFieldName(ExpressionHelper.GetExpressionText(expression));
        }

        public static string FieldIdFor<T, TResult>(this HtmlHelper<T> html, Expression<Func<T, TResult>> expression)
        {
            return html.ViewData.TemplateInfo.GetFullHtmlFieldId(ExpressionHelper.GetExpressionText(expression));
        }

        public static MvcHtmlString SelectOption<T>(this HtmlHelper html, T currentValue, T optionValue, string text)
        {
            return SelectOption(html, optionValue, object.Equals(optionValue, currentValue), text);
        }

        public static MvcHtmlString SelectOption(this HtmlHelper html, object optionValue, bool selected, string text)
        {
            var builder = new TagBuilder("option");

            if (optionValue != null)
                builder.MergeAttribute("value", optionValue.ToString());

            if (selected)
                builder.MergeAttribute("selected", "selected");

            builder.SetInnerText(text);

            return MvcHtmlString.Create(builder.ToString(TagRenderMode.Normal));
        }
       

        #region UnorderedList

        public static IHtmlString UnorderedList<T>(this HtmlHelper htmlHelper, IEnumerable<T> items, Func<T, int, MvcHtmlString> generateContent, string cssClass)
        {
            return htmlHelper.UnorderedList(items, generateContent, cssClass, null, null);
        }

        public static IHtmlString UnorderedList<T>(this HtmlHelper htmlHelper, IEnumerable<T> items, Func<T, int, MvcHtmlString> generateContent, string cssClass, string itemCssClass, string alternatingItemCssClass)
        {
            return UnorderedList(items, (t, i) => generateContent(t, i) as IHtmlString, cssClass, t => itemCssClass, t => alternatingItemCssClass);
        }

        public static IHtmlString UnorderedList<T>(this HtmlHelper htmlHelper, IEnumerable<T> items, Func<T, int, IHtmlString> generateContent, string cssClass)
        {
            return htmlHelper.UnorderedList(items, generateContent, cssClass, null, null);
        }

        public static IHtmlString UnorderedList<T>(this HtmlHelper htmlHelper, IEnumerable<T> items, Func<T, int, IHtmlString> generateContent, string cssClass, string itemCssClass, string alternatingItemCssClass)
        {
            return UnorderedList(items, generateContent, cssClass, t => itemCssClass, t => alternatingItemCssClass);
        }

        private static IHtmlString UnorderedList<T>(IEnumerable<T> items, Func<T, int, IHtmlString> generateContent, string cssClass, Func<T, string> generateItemCssClass, Func<T, string> generateAlternatingItemCssClass)
        {
            if (items == null || !items.Any()) return new HtmlString(string.Empty);

            var sb = new StringBuilder(250);
            int counter = 0, count = items.Count() - 1;

            sb.AppendFormat(
                !string.IsNullOrEmpty(cssClass) ? "<ul class=\"{0}\">" : "<ul>",
                cssClass
                );

            foreach (var item in items)
            {
                var sbClass = new StringBuilder(50);

                if (counter == 0)
                    sbClass.Append("first ");
                if (counter == count)
                    sbClass.Append("last ");
                if (generateItemCssClass != null)
                    sbClass.AppendFormat("{0} ", generateItemCssClass(item));
                if (counter % 2 != 0 && generateAlternatingItemCssClass != null)
                    sbClass.AppendFormat("{0} ", generateAlternatingItemCssClass(item));

                sb.AppendFormat(
                    sbClass.Length > 0
                        ? string.Format("<li class=\"{0}\">{{0}}</li>", sbClass.ToString().TrimEnd())
                        : "<li>{0}</li>",
                    generateContent(item, counter)
                    );

                counter++;
            }

            sb.Append("</ul>");

            return new HtmlString(sb.ToString());
        }

        #endregion


        #region Format Date/Time

        public static MvcHtmlString DateTime(this HtmlHelper htmlHelper, DateTime? value, MvcHtmlString defaultIfNull)
        {
            return value.HasValue ? htmlHelper.DateTime(value) : defaultIfNull;
        }

        public static MvcHtmlString DateTime(this HtmlHelper htmlHelper, DateTime? value, MvcHtmlString defaultIfNull, string customFormat)
        {
            return value.HasValue ? htmlHelper.DateTime(value, customFormat) : defaultIfNull;
        }

        public static MvcHtmlString LocalTime(this HtmlHelper htmlHelper, DateTime? value)
        {
            if (value.HasValue)
                return htmlHelper.DateTime(value.Value.ToLocalTime(), "yyyy-MM-dd");
            else
                return new MvcHtmlString(string.Empty);
        }

        public static MvcHtmlString LocalLongTime(this HtmlHelper htmlHelper, DateTime? value)
        {
            if (value.HasValue)
                return htmlHelper.DateTime(value.Value.ToLocalTime(), "yyyy-MM-dd hh:mm:ss");
            else
                return new MvcHtmlString(string.Empty);
        }

        public static MvcHtmlString DateTime(this HtmlHelper htmlHelper, DateTime? value)
        {
            if (value.HasValue)
                //TODO: (erikpo) This default format should come from a site setting
                return htmlHelper.DateTime(value.Value.ToLocalTime(), "MMM d yyyy h:mm tt"); //todo: above comment and get rid of just wrapping this as a localized string
            else
                return new MvcHtmlString(string.Empty);
        }

        public static MvcHtmlString DateTime(this HtmlHelper htmlHelper, DateTime? value, string customFormat)
        {
            if (value.HasValue)
                //TODO: (erikpo) In the future, convert this to "local" time before calling ToString
                return new MvcHtmlString(value.Value.ToString(customFormat));
            else
                return new MvcHtmlString(string.Empty);
        }

        #endregion

        #region Image

        public static MvcHtmlString Image(this HtmlHelper htmlHelper, string src, string alt, object htmlAttributes)
        {
            return htmlHelper.Image(src, alt, new RouteValueDictionary(htmlAttributes));
        }

        public static MvcHtmlString Image(this HtmlHelper htmlHelper, string src, string alt, IDictionary<string, object> htmlAttributes)
        {
            var url = new UrlHelper(htmlHelper.ViewContext.RequestContext);
            var imageUrl = url.Content(src);
            var imageTag = new TagBuilder("img");

            if (!string.IsNullOrEmpty(imageUrl))
                imageTag.MergeAttribute("src", imageUrl);

            if (!string.IsNullOrEmpty(alt))
                imageTag.MergeAttribute("alt", alt);

            imageTag.MergeAttributes(htmlAttributes, true);

            if (imageTag.Attributes.ContainsKey("alt") && !imageTag.Attributes.ContainsKey("title"))
                imageTag.MergeAttribute("title", imageTag.Attributes["alt"] ?? "");

            return MvcHtmlString.Create(imageTag.ToString(TagRenderMode.SelfClosing));
        }

        #endregion

        #region Link

        public static IHtmlString Link(this HtmlHelper htmlHelper, string linkContents, string href)
        {
            return htmlHelper.Link(linkContents, href, null);
        }

        public static IHtmlString Link(this HtmlHelper htmlHelper, string linkContents, string href, object htmlAttributes)
        {
            return htmlHelper.Link(linkContents, href, new RouteValueDictionary(htmlAttributes));
        }

        public static IHtmlString Link(this HtmlHelper htmlHelper, string linkContents, string href, IDictionary<string, object> htmlAttributes)
        {
            var tagBuilder = new TagBuilder("a") { InnerHtml = htmlHelper.Encode(linkContents) };
            tagBuilder.MergeAttributes(htmlAttributes);
            tagBuilder.MergeAttribute("href", href);
            return new HtmlString(tagBuilder.ToString(TagRenderMode.Normal));
        }

        #endregion

        #region LinkOrDefault

        public static IHtmlString LinkOrDefault(this HtmlHelper htmlHelper, string linkContents, string href)
        {
            return htmlHelper.LinkOrDefault(linkContents, href, null);
        }

        public static IHtmlString LinkOrDefault(this HtmlHelper htmlHelper, string linkContents, string href, object htmlAttributes)
        {
            return htmlHelper.LinkOrDefault(linkContents, href, new RouteValueDictionary(htmlAttributes));
        }

        public static IHtmlString LinkOrDefault(this HtmlHelper htmlHelper, string linkContents, string href, IDictionary<string, object> htmlAttributes)
        {
            string linkText = htmlHelper.Encode(linkContents);

            if (string.IsNullOrEmpty(href))
            {
                return new HtmlString(linkText);
            }

            var tagBuilder = new TagBuilder("a")
            {
                InnerHtml = linkText
            };
            tagBuilder.MergeAttributes(htmlAttributes);
            tagBuilder.MergeAttribute("href", href);
            return new HtmlString(tagBuilder.ToString(TagRenderMode.Normal));
        }

        #endregion

        #region BeginFormAntiForgeryPost

        public static MvcForm BeginFormAntiForgeryPost(this HtmlHelper htmlHelper)
        {
            return htmlHelper.BeginFormAntiForgeryPost(htmlHelper.ViewContext.HttpContext.Request.Url.PathAndQuery, FormMethod.Post, new RouteValueDictionary());
        }

        public static MvcForm BeginFormAntiForgeryPost(this HtmlHelper htmlHelper, object htmlAttributes)
        {
             return htmlHelper.BeginFormAntiForgeryPost(htmlHelper.ViewContext.HttpContext.Request.Url.PathAndQuery, FormMethod.Post,new RouteValueDictionary(htmlAttributes));
        }

        public static MvcForm BeginFormAntiForgeryPost(this HtmlHelper htmlHelper, string formAction)
        {
            return htmlHelper.BeginFormAntiForgeryPost(formAction, FormMethod.Post, new RouteValueDictionary());
        }

        public static MvcForm BeginFormAntiForgeryPost(this HtmlHelper htmlHelper, string formAction, FormMethod formMethod)
        {
            return htmlHelper.BeginFormAntiForgeryPost(formAction, formMethod, new RouteValueDictionary());
        }

        public static MvcForm BeginFormAntiForgeryPost(this HtmlHelper htmlHelper, string formAction, FormMethod formMethod, object htmlAttributes)
        {
            return htmlHelper.BeginFormAntiForgeryPost(formAction, formMethod, new RouteValueDictionary(htmlAttributes));
        }

        

        public static MvcForm BeginFormAntiForgeryPost(this HtmlHelper htmlHelper, string formAction, FormMethod formMethod, IDictionary<string, object> htmlAttributes)
        {
            // Force the browser not to cache protected forms, and to reload them if needed.
            var response = htmlHelper.ViewContext.HttpContext.Response;
            response.Cache.SetExpires(System.DateTime.UtcNow.AddDays(-1));
            response.Cache.SetValidUntilExpires(false);
            response.Cache.SetRevalidation(HttpCacheRevalidation.AllCaches);
            response.Cache.SetCacheability(HttpCacheability.NoCache);
            response.Cache.SetNoStore();

            var tagBuilder = new TagBuilder("form");

            tagBuilder.MergeAttributes(htmlAttributes);
            tagBuilder.MergeAttribute("action", formAction);
            tagBuilder.MergeAttribute("method", HtmlHelper.GetFormMethodString(formMethod), true);

            htmlHelper.ViewContext.Writer.Write(tagBuilder.ToString(TagRenderMode.StartTag));

            return new MvcFormAntiForgeryPost(htmlHelper);
        }

        #endregion


        #region AntiForgeryTokenUSO

        public static MvcHtmlString AntiForgeryTokenUSO(this HtmlHelper htmlHelper)
        {
          

            try
            {
                //return htmlHelper.AntiForgeryToken(siteSalt);  //for mvc 3 
                return htmlHelper.AntiForgeryToken();  //for mvc 4
            }
            catch (HttpAntiForgeryException)
            {
                // Work-around an issue in MVC 2:  If the browser sends a cookie that is not
                // coming from this server (this can happen if the user didn't close their browser
                // while the application server configuration changed), clear it up
                // so that a new one is generated and sent to the browser. This is harmless
                // from a security point of view, since we are _issuing_ an anti-forgery token,
                // not validating input.

                // Remove the token so that MVC will create a new one.
                var antiForgeryTokenName = htmlHelper.GetAntiForgeryTokenName();
                htmlHelper.ViewContext.HttpContext.Request.Cookies.Remove(antiForgeryTokenName);

                // Try again
                //return htmlHelper.AntiForgeryToken(siteSalt);//for mvc 3
                return htmlHelper.AntiForgeryToken();          //for mvc 4
            }
        }

        private static string GetAntiForgeryTokenName(this HtmlHelper htmlHelper)
        {
            // Generate the same cookie name as MVC
            var appPath = htmlHelper.ViewContext.HttpContext.Request.ApplicationPath;
            const string antiForgeryTokenName = "__RequestVerificationToken";
            if (string.IsNullOrEmpty(appPath))
            {
                return antiForgeryTokenName;
            }
            return antiForgeryTokenName + '_' + Base64EncodeForCookieName(appPath);
        }

        private static string Base64EncodeForCookieName(string s)
        {
            return Convert.ToBase64String(Encoding.UTF8.GetBytes(s)).Replace('+', '.').Replace('/', '-').Replace('=', '_');
        }

        #endregion

        #region AntiForgeryTokenValueUSOLink

        public static IHtmlString AntiForgeryTokenValueUSOLink(this HtmlHelper htmlHelper, string linkContents, string href)
        {
            return htmlHelper.AntiForgeryTokenValueUSOLink(linkContents, href, (object)null);
        }

        public static IHtmlString AntiForgeryTokenValueUSOLink(this HtmlHelper htmlHelper, string linkContents, string href, object htmlAttributes)
        {
            return htmlHelper.AntiForgeryTokenValueUSOLink(linkContents, href, new RouteValueDictionary(htmlAttributes));
        }

        public static IHtmlString AntiForgeryTokenValueUSOLink(this HtmlHelper htmlHelper, string linkContents, string href, IDictionary<string, object> htmlAttributes)
        {
            return htmlHelper.Link(linkContents, htmlHelper.AntiForgeryTokenGetUrl(href).ToString(), htmlAttributes);
        }

        #endregion

        #region AntiForgeryTokenGetUrl

        public static IHtmlString AntiForgeryTokenGetUrl(this HtmlHelper htmlHelper, string baseUrl)
        {
            return new HtmlString(string.Format("{0}{1}__RequestVerificationToken={2}", baseUrl, baseUrl.IndexOf('?') > -1 ? "&" : "?", htmlHelper.ViewContext.HttpContext.Server.UrlEncode(htmlHelper.AntiForgeryTokenValueUSO().ToString())));
        }

        #endregion


        #region AntiForgeryTokenValueUSO

        public static IHtmlString AntiForgeryTokenValueUSO(this HtmlHelper htmlHelper)
        {
            //HAACK: (erikpo) Since MVC doesn't expose any of its methods for generating the antiforgery token and setting the cookie, we'll just let it do its thing and parse out what we need
            var field = htmlHelper.AntiForgeryTokenUSO().ToHtmlString();
            var beginIndex = field.IndexOf("value=\"") + 7;
            var endIndex = field.IndexOf("\"", beginIndex);

            return new HtmlString(field.Substring(beginIndex, endIndex - beginIndex));
        }

        #endregion     


        #region
        public static MvcHtmlString CustomValidationMessage(this HtmlHelper instance, string modelName)
        {
            Check.Argument.IsNotNull(instance, "instance");
            Check.Argument.IsNotNull(modelName, "modelName");

            var builder = new TagBuilder("span");
            builder.MergeAttribute("class", HtmlHelper.ValidationMessageCssClassName);

            if (instance.ViewData.ModelState.ContainsKey(modelName))
            {
                var modelState = instance.ViewData.ModelState[modelName];
                var modelErrors = (modelState == null) ? null : modelState.Errors;
                var modelError = ((modelErrors == null) || (modelErrors.Count == 0)) ? null : modelErrors[0];

                if (modelError != null)
                {
                    var validationMessage = GetValidationMessage(modelError);

                    var iconBuilder = new TagBuilder("img");

                    iconBuilder.MergeAttribute("src", instance.ViewContext.RequestContext.UrlHelper().InputValidationErrorIcon());
                    iconBuilder.MergeAttribute("alt", string.Empty);
                    iconBuilder.MergeAttribute("title", validationMessage);

                    builder.InnerHtml = iconBuilder.ToString(TagRenderMode.SelfClosing);
                }
            }

            return MvcHtmlString.Create(builder.ToString());
        }

        public static MvcHtmlString CustomValidationMessageNoICON(this HtmlHelper instance, string modelName, string validationMessageCssClassName)
        {
            Check.Argument.IsNotNull(instance, "instance");
            Check.Argument.IsNotNull(modelName, "modelName");

            var builder = new TagBuilder("span");

            if (string.IsNullOrWhiteSpace(validationMessageCssClassName))
                validationMessageCssClassName = HtmlHelper.ValidationMessageCssClassName;

            builder.MergeAttribute("class", validationMessageCssClassName);

            if (instance.ViewData.ModelState.ContainsKey(modelName))
            {
                var modelState = instance.ViewData.ModelState[modelName];
                var modelErrors = (modelState == null) ? null : modelState.Errors;
                var modelError = ((modelErrors == null) || (modelErrors.Count == 0)) ? null : modelErrors[0];

                if (modelError != null)
                {
                    var validationMessage = GetValidationMessage(modelError);
                    builder.InnerHtml = validationMessage;
                }
            }

            return MvcHtmlString.Create(builder.ToString());
        }

        public static MvcHtmlString CustomValidationSummaryWithAlert(this HtmlHelper instance)
        {
            Check.Argument.IsNotNull(instance, "instance");
            var script = string.Empty;
            var errors = string.Empty;
            if (instance.ViewData.ModelState != null && instance.ViewData.ModelState.Count > 0)
            {
                foreach (var pair in instance.ViewData.ModelState)
                {
                    foreach (var modelError in pair.Value.Errors)
                    {
                        var errorText = GetValidationMessage(modelError);
                        errors += "* " + errorText + @"\n";
                    }
                }
                if (errors == "")
                    return MvcHtmlString.Empty;
                script = string.Format("<script>alert(\"{0}\")</script>", errors.Replace("\"", "\\\""));

                return MvcHtmlString.Create(script);
            }
            return MvcHtmlString.Empty;
        }

        public static MvcHtmlString CustomValidationSummary(this HtmlHelper instance, string message, object htmlAttributes)
        {
            Check.Argument.IsNotNull(instance, "instance");

            var innerHtml = string.Empty;

            if (!instance.ViewData.ModelState.IsValid)
            {
                var spanTag = new TagBuilder("span");
                spanTag.MergeAttribute("class", HtmlHelper.ValidationSummaryCssClassName);
                spanTag.SetInnerText(message);
                var messageSpan = spanTag + Environment.NewLine;

                var htmlSummary = new StringBuilder();
                var unorderedList = new TagBuilder("ul");
                unorderedList.MergeAttribute("class", HtmlHelper.ValidationSummaryCssClassName);

                foreach (var pair in instance.ViewData.ModelState)
                {
                    foreach (var modelError in pair.Value.Errors)
                    {
                        string errorText = GetValidationMessage(modelError);

                        var label = new TagBuilder("label");
                        label.MergeAttribute("for", pair.Key);
                        label.SetInnerText(errorText);

                        var listItem = new TagBuilder("li") { InnerHtml = label.ToString() };

                        htmlSummary.AppendLine(listItem.ToString());
                    }
                }

                unorderedList.InnerHtml = htmlSummary.ToString();

                innerHtml = messageSpan + unorderedList;
            }

            var boxBuilder = new TagBuilder("div");
            boxBuilder.MergeAttributes(new RouteValueDictionary(htmlAttributes));
            boxBuilder.AddCssClass("validationBox");
            boxBuilder.InnerHtml = innerHtml;

            if (instance.ViewData.ModelState.IsValid)
            {
                var style = boxBuilder.Attributes.ContainsKey("style") ?
                               boxBuilder.Attributes["style"] :
                               string.Empty;

                style += (!string.IsNullOrWhiteSpace(style) ? ";" : string.Empty) + "display:none";

                boxBuilder.Attributes["style"] = style;
            }

            return MvcHtmlString.Create(boxBuilder.ToString());
        }

        public static ListHtmlHelper List(this HtmlHelper htmlHelper)
        {
            return new ListHtmlHelper(htmlHelper);
        }

        public static PagerHtmlHelper Pager(this HtmlHelper htmlHelper)
        {
            return new PagerHtmlHelper(htmlHelper);
        }

        public static string DropDownList<TEnum>(this HtmlHelper instance, string name, TEnum selectedValue) where TEnum : IComparable, IFormattable, IConvertible
        {
            var enumType = typeof(TEnum);

            var names = Enum.GetNames(enumType);
            var values = Enum.GetValues(enumType);

            var list = new List<SelectListItem>();

            for (var i = 0; i < names.Length; i++)
            {
                var text = names[i];
                var value = values.GetValue(i);

                list.Add(new SelectListItem { Text = text, Value = value.ToString(), Selected = value.Equals(selectedValue) });
            }

            return instance.DropDownList(name, list.OrderBy(i => i.Text), (string)null).ToString();
        }

        private static string GetValidationMessage(ModelError modelError)
        {
            var validationMessage = !string.IsNullOrEmpty(modelError.ErrorMessage) ?
                                        modelError.ErrorMessage :
                                        ((modelError.Exception != null) ? modelError.Exception.Message : "Error");

            return validationMessage;
        }
        #endregion

        public static MvcHtmlString FormatPartNumber(this HtmlHelper html, string partNumber, string key)
        {
            if (string.IsNullOrWhiteSpace(key))
            {
                return new MvcHtmlString(partNumber);
            }

            var prefix = string.Empty;
            var partBase = string.Empty;
            var partitionSupport = "-1";

            ParsePartNumber(key, ref prefix, ref partBase, ref partitionSupport);

            var patternExact = string.Empty;
            var patternBase = string.Empty;

            foreach (var c in key)
            {
                if (c == '+' ) continue;

                patternExact += @"\W*" + c;
            }

            if (partBase != string.Empty)
            {
                foreach (var c in partBase)
                {
                    patternBase += @"\W*" + c;
                }
            }

            var regexExact = new Regex(patternExact, RegexOptions.IgnoreCase);

            var match = regexExact.Match(partNumber);
            if (match.Success)
            {
                return new MvcHtmlString(partNumber.Substring(0, match.Index) + "<b><font color='red'>" + match.Value + "</font></b>" + partNumber.Substring(match.Index + match.Value.Length));
            }
            else if (partBase != string.Empty)
            {
                var regexBase = new Regex(patternBase, RegexOptions.IgnoreCase);

                var m = regexBase.Match(partNumber);
                if (m.Success)
                {
                    return new MvcHtmlString(partNumber.Substring(0, m.Index) + "<b><font color='red'>" + m.Value + "</font></b>" + partNumber.Substring(m.Index + m.Value.Length));
                }
                else
                {
                    return new MvcHtmlString(partNumber);
                }
            }
            else
            {
                return new MvcHtmlString(partNumber);
            }
        }

        private static void ParsePartNumber(string partNumber, ref string prefix, ref string partBase, ref string partitionSupport)
        {
            if (string.IsNullOrEmpty(partNumber))
            {
                prefix = string.Empty;
                partBase = string.Empty;
                //partitionSupport = string.Empty;   bn注释 ,采用下面getPartitionSupportByPartNumber方法取值
                return;
            }
            var regex = new Regex("[0-9]");
            var match = regex.Match(partNumber);
            if (match.Success)
            {
                prefix = partNumber.Substring(0, match.Index);
                partBase = partNumber.Substring(match.Index);
                //partitionSupport = partNumber.Substring(match.Index, 1); bn注释 ,采用下面getPartitionSupportByPartNumber方法取值
            }
            else
            {
                prefix = partNumber;
                //partitionSupport = "-1"; bn注释 ,采用下面getPartitionSupportByPartNumber方法取值
            }
           // partitionSupport = getPartitionSupportByPartNumber(partNumber);

        }

    }
}
