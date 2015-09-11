namespace USO.Core
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Web.Mvc;
    using MvcExtensions;
    using USO.Core.Events;
    using USO.Core.Logging;
    using USO.Core.Logging.Infrastructure;

    /// <summary>
    /// Defines a class which is used to register the default <seealso cref="IDependency"/>.
    /// </summary>
    //[DependsOn(typeof(RegisterDefaultUsers))]
    //[DependsOn("USO.Security.RegisterDefaultUsers, USO.Framework")]
    public class RegisterDependencies : BootstrapperTask
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RegisterModelMetadata"/> class.
        /// </summary>
        /// <param name="container">The container.</param>
        public RegisterDependencies(ContainerAdapter container)
        {
            Invariant.IsNotNull(container, "container");

            Container = container;
        }

        /// <summary>
        /// Gets the container.
        /// </summary>
        /// <value>The container.</value>
        protected ContainerAdapter Container { get; private set; }

        /// <summary>
        /// Executes the task. Returns continuation of the next task(s) in the chain.
        /// </summary>
        /// <returns></returns>
        public override TaskContinuation Execute()
        {
            // by default, use USO.Core's logger that delegates to Ninject's logger factory
            Container.RegisterAsSingleton(typeof (ILoggerFactory), typeof (Log4NetLoggerFactory));

            var loggerFactory = Container.GetService<ILoggerFactory>();
            var logger = loggerFactory.CreateLogger(typeof (RegisterDependencies));
            // call CreateLogger in response to the request for an ILogger implementation
            Container.RegisterInstance(null, typeof (ILogger), logger);

            IBuildManager buildManager = Container.GetService<IBuildManager>();

            Func<Type, bool> IsDependency = type => typeof (IDependency).IsAssignableFrom(type);
          
            buildManager
                .ConcreteTypes
                .Where(IsDependency)
                .Each(type =>
                    {
                        foreach (var interfaceType in type.GetInterfaces()
                                                          .Where(itf => typeof (IDependency).IsAssignableFrom(itf)
                                                                        && !typeof (IEventHandler).IsAssignableFrom(itf))
                            )
                        {
                            if (typeof (ISingletonDependency).IsAssignableFrom(interfaceType))
                            {
                                Container.RegisterAsSingleton(interfaceType, type);
                            }
                            else if (typeof (IUnitOfWorkDependency).IsAssignableFrom(interfaceType))
                            {
                                Container.RegisterAsSingleton(interfaceType, type);
                            }
                            else if (typeof (ITransientDependency).IsAssignableFrom(interfaceType))
                            {
                                Container.RegisterAsTransient(interfaceType, type);
                            }
                            else if (typeof (IPerRequestDependency).IsAssignableFrom(interfaceType))
                                Container.RegisterAsPerRequest(interfaceType, type);
                            else
                                Container.RegisterAsPerRequest(interfaceType, type);
                        }

                        if (typeof (IEventHandler).IsAssignableFrom(type))
                        {
                            //用于非事件总线执行
                            foreach (var interfaceType in type.GetInterfaces()
                                                              .Where(itf => itf != typeof (IEventHandler)))
                            {
                                Container.RegisterAsPerRequest(interfaceType, type);
                            }
                            //用于事件总线执行
                            //Container.RegisterAsTransient(typeof(IEventHandler), type);
                        }
                    }
                );

            //注册WCF Rest Resources
            //Func<Type, bool> IsResourceBase = type => typeof(ResourceBase).IsAssignableFrom(type);

            //buildManager
            //         .ConcreteTypes
            //         .Where(IsResourceBase)
            //         .Each(type => Container.RegisterAsTransient(type));

            return TaskContinuation.Continue;
        }
    }
}