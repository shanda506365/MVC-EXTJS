
namespace USO.Mvc.Filters
{
    using System;
    using System.Linq;
    using System.Web.Mvc;
    using MvcExtensions;

    /// <summary>
    /// Defines a class which is used to register available <seealso cref="IGlobalFilterProvider"/>.
    /// </summary>
    public class RegisterGlobalFilterProviders : IgnorableTypesBootstrapperTask<RegisterGlobalFilterProviders, IGlobalFilterProvider>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RegisterFilterProviders"/> class.
        /// </summary>
        /// <param name="container">The container.</param>
        public RegisterGlobalFilterProviders(ContainerAdapter container)
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
        /// Executes the task.
        /// </summary>
        /// <returns></returns>
        public override TaskContinuation Execute()
        {
            Func<Type, bool> filter = type => typeof(IGlobalFilterProvider).IsAssignableFrom(type) &&
                                              type.Assembly != KnownAssembly.AspNetMvcAssembly &&
                                              !type.Assembly.GetName().Name.Equals(KnownAssembly.AspNetMvcFutureAssemblyName, StringComparison.OrdinalIgnoreCase) &&
                                              !IgnoredTypes.Any(ignoredType => ignoredType == type);

            Container.GetService<IBuildManager>()
                     .ConcreteTypes
                     .Where(filter)
                     .Each(type => Container.RegisterAsTransient(typeof(IGlobalFilterProvider), type));

            return TaskContinuation.Continue;
        }
    }
}