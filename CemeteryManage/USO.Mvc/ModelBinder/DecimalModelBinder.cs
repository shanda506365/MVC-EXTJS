
namespace USO.Mvc.ModelBinder
{
    using System;
    using System.Web.Mvc;

    public class DecimalModelBinder : DefaultModelBinder
    {
        public override object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            var valueProviderResult = bindingContext.ValueProvider.GetValue(bindingContext.ModelName);
            if (valueProviderResult == null)
                return base.BindModel(controllerContext, bindingContext);

            if (valueProviderResult.AttemptedValue.Equals("N.aN") ||
                valueProviderResult.AttemptedValue.Equals("NaN") ||
                valueProviderResult.AttemptedValue.Equals("Infini.ty") ||
                valueProviderResult.AttemptedValue.Equals("Infinity"))
                return 0m;

            if (string.IsNullOrEmpty(valueProviderResult.AttemptedValue))
                return null;

            return Convert.ToDecimal(valueProviderResult.AttemptedValue);
        }
    }
}
