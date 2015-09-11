namespace MvcExtensions
{
    using System;
    using System.Web.Mvc;
    using USO.Mvc.Filters;

    internal static class KnownTypes
    {
        public static readonly Type BootstrapperTaskType = typeof(BootstrapperTask);

        public static readonly Type PerRequestTaskType = typeof(PerRequestTask);

        public static readonly Type ModelBinderType = typeof(IModelBinder);

        public static readonly Type ControllerActivatorType = typeof(IControllerActivator);

        public static readonly Type ControllerType = typeof(Controller);

        public static readonly Type ActionInvokerType = typeof(IActionInvoker);

        //public static readonly Type SyncActionInvokerType = typeof(ExtendedControllerActionInvoker);

        //public static readonly Type AsyncActionInvokerType = typeof(ExtendedAsyncControllerActionInvoker);

        public static readonly Type SyncActionInvokerType = typeof(GlobalFilterResolvingActionInvoker);// typeof(ExtendedControllerActionInvoker);

        public static readonly Type AsyncActionInvokerType = typeof(GlobalFilterResolvingAsyncActionInvoker);// typeof(ExtendedAsyncControllerActionInvoker);

        public static readonly Type FilterType = typeof(IMvcFilter);

        public static readonly Type FilterAttributeType = typeof(FilterAttribute);

        public static readonly Type FilterProviderType = typeof(IFilterProvider);

        public static readonly Type ViewPageActivatorType = typeof(IViewPageActivator);

        public static readonly Type ViewType = typeof(IView);

        public static readonly Type ViewEngineType = typeof(IViewEngine);

        public static readonly Type ActionResultType = typeof(ActionResult);

        public static readonly Type ValueProviderFactoryType = typeof(ValueProviderFactory);

        public static readonly Type ModelMetadataConfigurationType = typeof(IModelMetadataConfiguration);
    }
}
