
namespace USO.Mvc
{
    using System;
    using System.Diagnostics;
    using System.Linq;
    using System.Web.Mvc;
    using MvcExtensions;

    /// <summary>
    /// Defines a class which is used to register the default <seealso cref="IControllerActivator"/>.
    /// </summary>
    public class RegisterAreaControllerActivator : BootstrapperTask
    {
        private Type controllerActivatorType = typeof(USOControllerActivator);

        /// <summary>
        /// Initializes a new instance of the <see cref="RegisterControllerActivator"/> class.
        /// </summary>
        /// <param name="container">The container.</param>
        public RegisterAreaControllerActivator(ContainerAdapter container)
        {
            Invariant.IsNotNull(container, "container");

            Container = container;
        }

        /// <summary>
        /// Gets or sets the type of the controller activator.
        /// </summary>
        /// <value>The type of the controller activator.</value>
        public Type ControllerActivatorType
        {
            [DebuggerStepThrough]
            get
            {
                return controllerActivatorType;
            }

            [DebuggerStepThrough]
            set
            {
                controllerActivatorType = value;
            }
        }

        /// <summary>
        /// Gets the container.
        /// </summary>
        /// <value>The container.</value>
        protected ContainerAdapter Container
        {
            get;
            private set;
        }

        /// <summary>
        /// Executes the task.
        /// </summary>
        /// <returns></returns>
        public override TaskContinuation Execute()
        {
            Func<Type, bool> filter = type => KnownTypes.ControllerActivatorType.IsAssignableFrom(type) &&
                                              type != ControllerActivatorType &&
                                              type.Assembly != KnownAssembly.AspNetMvcAssembly &&
                                              !type.Assembly.GetName().Name.Equals(KnownAssembly.AspNetMvcFutureAssemblyName, StringComparison.OrdinalIgnoreCase);

            Container.GetService<IBuildManager>()
                     .ConcreteTypes
                     .Where(filter)
                     .Each(type => Container.RegisterAsSingleton(type));

            Container.RegisterAsSingleton(KnownTypes.ControllerActivatorType, ControllerActivatorType);

            return TaskContinuation.Continue;
        }
    }
}