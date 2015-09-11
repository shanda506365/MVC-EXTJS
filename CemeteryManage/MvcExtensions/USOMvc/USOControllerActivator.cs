
namespace USO.Mvc
{
    using System;
    using System.Web.Mvc;
    using System.Web.Mvc.Async;
    using System.Web.Routing;
    using MvcExtensions;

    /// <summary>
    /// The Default IoC backed <seealso cref="ExtendedControllerActivator"/>.
    /// </summary>
    public class USOControllerActivator : ExtendedControllerActivator
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="USOControllerActivator"/> class.
        /// </summary>
        /// <param name="container">The container.</param>
        /// <param name="controllerActivatorRegistry">The controller activator registry.</param>
        /// <param name="actionInvokerRegistry">The action invoker registry.</param>
        public USOControllerActivator(ContainerAdapter container, TypeMappingRegistry<Controller, IControllerActivator> controllerActivatorRegistry, TypeMappingRegistry<Controller, IActionInvoker> actionInvokerRegistry)
            : base(container, controllerActivatorRegistry, actionInvokerRegistry)
        {
        }
      

        /// <summary>
        /// Creates the specified request context.
        /// </summary>
        /// <param name="requestContext">The request context.</param>
        /// <param name="controllerType">Type of the controller.</param>
        /// <returns></returns>
        public override IController Create(RequestContext requestContext, Type controllerType)
        {
            if (controllerType == null)
            {
                return null;
            }

            Type activatorType = ControllerActivatorRegistry.Matching(controllerType);

            IControllerActivator activator = activatorType != null ?
                                             (IControllerActivator)Container.GetServices(activatorType) :
                                             null;

            Controller controller = activator != null ?
                                    activator.Create(requestContext, controllerType) as Controller :
                                    (GetControllerInstance(controllerType) ?? Activator.CreateInstance(controllerType)) as Controller;

            if (controller != null)
            {
                Type actionInvokerType;

                if (ActionInvokerRegistry.IsRegistered(controllerType))
                {
                    actionInvokerType = ActionInvokerRegistry.Matching(controllerType);
                }
                else
                {
                    actionInvokerType = controller is IAsyncController ?
                                        KnownTypes.AsyncActionInvokerType :
                                        KnownTypes.SyncActionInvokerType;
                }

                IActionInvoker actionInvoker = Container.GetService(actionInvokerType) as IActionInvoker;

                if (actionInvoker != null)
                {
                    controller.ActionInvoker = actionInvoker;
                }
            }

            return controller;
        }

        private Controller GetControllerInstance(Type controllerType)
        {
            string name = controllerType.Name;
            if (name.EndsWith("Controller"))
            {
                name = name.Substring(0, name.Length - "Controller".Length);
            }
            string key = (controllerType.Assembly.GetName().Name + "/" + name).ToLowerInvariant();

            var service = Container.GetService<Controller>(key);
            if (service != null)
            {
                return service;
            }
            return null;
        }
    }
}