namespace USO.Mvc.BootstrapperTasks
{
    using MvcExtensions;
    using Telerik.Web.Mvc;

    public class ConfigureAssets : BootstrapperTask
    {
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
        /// Initializes a new instance of the <see cref="ConfigureAssets"/> class.
        /// </summary>
        /// <param name="container">The container.</param>
        public ConfigureAssets(ContainerAdapter container)
        {
            Invariant.IsNotNull(container, "container");

            Container = container;
        }

        public override TaskContinuation Execute()
        {
            WebAssetDefaultSettings.UseTelerikContentDeliveryNetwork = false;
            WebAssetDefaultSettings.Combined = false;
            WebAssetDefaultSettings.Compress = true;
            WebAssetDefaultSettings.CacheDurationInDays = 1;
           
            SharedWebAssets.StyleSheets(
                group => group.AddGroup(
                    "USOControlPanelStyles",
                    styles =>
                    styles.Add("ControlPanel/common.css")
                        .Add("2013.1.219/telerik.common.min.css")
                        .Add("2013.1.219/telerik.office2007.min.css")
                             ));
           
            SharedWebAssets.Scripts(
                group => group.AddGroup(
                    "USOControlPanelScripts",
                    scripts =>
                    scripts
                        .Add("jquery.cookie.js")
                        .Add("jquery.form.js")
                        .Add("2013.1.219/telerik.common.min.js")
                        .Add("2013.1.219/telerik.menu.min.js")
                        .Add("2013.1.219/telerik.tabstrip.min.js")
                        .Add("2013.1.219/telerik.upload.min.js")
                        .Add("2013.1.219/telerik.calendar.min.js")
                         .Add("ControlPanel/common.js")));

            SharedWebAssets.Scripts(
               group => group.AddGroup(
                   "USOFrameScripts",
                   scripts =>
                   scripts
                       .Add("2013.1.219/telerik.common.min.js")
                       .Add("2013.1.219/telerik.menu.min.js")
                       .Add("2013.1.219/telerik.tabstrip.min.js")
                       .Add("ControlPanel/common.js")
                       ));

            SharedWebAssets.Scripts(
             group => group.AddGroup(
                 "USOControlPanelStage",
                 scripts =>
                 scripts
                     .Add("jquery.cookie.js")
                     .Add("jquery.form.js")
                     .Add("2013.1.219/telerik.common.min.js")
                     .Add("2013.1.219/telerik.menu.min.js")
                     .Add("2013.1.219/telerik.tabstrip.min.js")
                     .Add("2013.1.219/telerik.upload.min.js")
                     .Add("ControlPanel/common.js")));

            SharedWebAssets.Scripts(
            group => group.AddGroup(
                "xxx",
                scripts =>
                scripts
                    .Add("2013.1.219/telerik.tabstrip.min.js")));

         

            return TaskContinuation.Continue;             
        }
    }
}
