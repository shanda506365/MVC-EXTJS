
namespace USO.Mvc.Html
{
    using System;
    using System.Linq.Expressions;
    using System.Web.Mvc;
    using Telerik.Web.Mvc.UI;
    using USO.Domain;

    public static class TelerikExtensions
    {
        public static IntegerTextBoxBuilder IntegerTextBoxFor<TModel, TProperty>(this HtmlHelper<TModel> instance, Expression<Func<TModel, TProperty>> expression)
        {
          

            var name = ExpressionHelper.GetExpressionText(expression);
            return instance.Telerik().IntegerTextBox().Name(name);
        }

        //[DebuggerStepThrough]
        //public virtual NumericTextBoxBuilder<T> NumericTextBox<T>() where T : struct
        //{
        //    return new NumericTextBoxBuilder<T>(this.Create<NumericTextBox<T>>(() => new NumericTextBox<T>(this.ViewContext, this.ClientSideObjectWriterFactory, ServiceLocator.Current.Resolve<ITextboxBaseHtmlBuilderFactory<T>>())));
        //}

        public static NumericTextBoxBuilder<T> NumericTextBoxFor<TModel, TProperty, T>(this HtmlHelper<TModel> instance, Expression<Func<TModel, TProperty>> expression) where T : struct
        {
           

            var name = ExpressionHelper.GetExpressionText(expression);
            name = instance.ViewContext.ViewData.TemplateInfo.GetFullHtmlFieldName(name);
            return instance.Telerik().NumericTextBox<T>().Name(name);
        }

        public static NumericTextBoxBuilder<double> NumericTextBoxFor<TModel, TProperty>(this HtmlHelper<TModel> instance, Expression<Func<TModel, TProperty>> expression)
        {

            var name = ExpressionHelper.GetExpressionText(expression);
            name = instance.ViewContext.ViewData.TemplateInfo.GetFullHtmlFieldName(name);
            return instance.Telerik().NumericTextBox<double>().Name(name);
        }
    }
}
