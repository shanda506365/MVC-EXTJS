namespace USO.Mvc
{
    using System;
    using System.Linq;
    using System.Web.Mvc;
    using MvcExtensions;
    using System.Diagnostics;

    /// <summary>
    /// Defines a class which is used to register available <seealso cref="Controller"/>.
    /// </summary>
    [DependsOn(typeof(RegisterAreaControllerActivator))]
    public class RegisterAreaControllers : IgnorableTypesBootstrapperTask<RegisterAreaControllers, Controller>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RegisterAreaControllers"/> class.
        /// </summary>
        /// <param name="container">The container.</param>
        public RegisterAreaControllers(ContainerAdapter container)
        {
            Invariant.IsNotNull(container, "container");

            Container = container;
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
        /// Executes the task. Returns continuation of the next task(s) in the chain.
        /// </summary>
        /// <returns></returns>
        public override TaskContinuation Execute()
        {
            Func<Type, bool> filter = type => KnownTypes.ControllerType.IsAssignableFrom(type) &&
                                              type.Assembly != KnownAssembly.AspNetMvcAssembly &&
                                              !type.Assembly.GetName().Name.Equals(KnownAssembly.AspNetMvcFutureAssemblyName, StringComparison.OrdinalIgnoreCase) &&
                                              !IgnoredTypes.Any(ignoredType => ignoredType == type);
            
            Container.GetService<IBuildManager>()
                     .ConcreteTypes
                     .Where(filter)
                     .Each(type => {
                         var controllerName = type.Name;
                         if (controllerName.EndsWith("Controller"))
                             controllerName = controllerName.Substring(0, controllerName.Length - "Controller".Length);

                         var areaName = type.Assembly.GetName().Name;

                         var serviceKey = (areaName + "/" + controllerName).ToLowerInvariant();
                         Container.RegisterType(serviceKey, KnownTypes.ControllerType, type, LifetimeType.Transient);                  
                     });

            return TaskContinuation.Continue;
        }
    }
}