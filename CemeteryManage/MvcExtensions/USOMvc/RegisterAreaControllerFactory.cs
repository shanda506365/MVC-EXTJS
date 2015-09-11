namespace USO.Mvc
{
    using MvcExtensions;
    using System;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;
    using System.Web.Mvc;

    /// <summary>
    /// Defines a class which is used to register the default <seealso cref="T:System.Web.Mvc.IControllerFactory" />.
    /// </summary>
    public class RegisterAreaControllerFactory : BootstrapperTask
    {
        private static Type controllerFactoryType = typeof(AreaControllerFactory);

        /// <summary>
        /// Gets the container.
        /// </summary>
        /// <value>The container.</value>
        protected ContainerAdapter Container { get; private set; }

        /// <summary>
        /// Gets or sets the type of the controller factory.
        /// </summary>
        /// <value>The type of the controller factory.</value>
        public static Type ControllerFactoryType
        {
            [DebuggerStepThrough]
            get
            {
                return controllerFactoryType;
            }
            [DebuggerStepThrough]
            set
            {
                controllerFactoryType = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="T:MvcExtensions.USO.RegisterAreaControllerFactory" /> should be excluded.
        /// </summary>
        /// <value><c>true</c> if excluded; otherwise, <c>false</c>.</value>
        public static bool Excluded { get; set; }



        /// <summary>
        /// Initializes a new instance of the <see cref="T:MvcExtensions.USO.RegisterAreaControllerFactory" /> class.
        /// </summary>
        /// <param name="container">The container.</param>
        public RegisterAreaControllerFactory(ContainerAdapter container)
        {
            Invariant.IsNotNull(container, "container");
            this.Container = container;
        }

        /// <summary>
        /// Executes the task. Returns continuation of the next task(s) in the chain.
        /// </summary>
        /// <returns></returns>
        public override TaskContinuation Execute()
        {
            if (!Excluded)
            {
                ControllerBuilder.Current.SetControllerFactory(new AreaControllerFactory(this.Container));
            }
            return TaskContinuation.Continue;
        }




    }
}
