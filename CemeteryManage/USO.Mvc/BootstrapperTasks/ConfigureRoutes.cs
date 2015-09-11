namespace USO.Mvc.BootstrapperTasks
{
    using System.Linq;
    using System.Web.Mvc;
    using System.Web.Routing;
    using MvcExtensions;
    using USO.Mvc.Routes;

    public class ConfigureRoutes : RegisterRoutesBase
    {
        public ConfigureRoutes(ContainerAdapter container, RouteCollection routes)
            : base(routes)
        {
            Invariant.IsNotNull(container, "container");
            Container = container;
        }

        /// <summary>
        /// Gets the container.
        /// </summary>
        /// <value>The container.</value>
        private ContainerAdapter Container
        {
            get;
            set;
        }

        protected override void Register()
        {
            Routes.Clear();

            // Turns off the unnecessary file exists check
            Routes.RouteExistingFiles = false;

            // Ignore axd files
            Routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            Routes.IgnoreRoute("WebForms/{weform}");

            // Ignore known static files
            Routes.IgnoreRoute("{file}.txt");
            Routes.IgnoreRoute("{file}.htm");
            Routes.IgnoreRoute("{file}.html");
            Routes.IgnoreRoute("{file}.xml");

            //Ignore the assets directory which contains css, images and js
            Routes.IgnoreRoute("Content/{*pathInfo}");
            Routes.IgnoreRoute("Scripts/{*pathInfo}");
            Routes.IgnoreRoute("Resource/{*pathInfo}");

            Routes.IgnoreRoute("{*favicon}", new { favicon = new RegexConstraint(@"(.*/)?favicon.([iI][cC][oO]|[gG][iI][fF])(/.*)?") });

            AreaRegistration.RegisterAllAreas();

            var providers = Container.GetServices<IRouteProvider>();
            Container.GetService<IRoutePublisher>().Publish(providers.SelectMany(p => p.GetRoutes()));
        }
    }

}
