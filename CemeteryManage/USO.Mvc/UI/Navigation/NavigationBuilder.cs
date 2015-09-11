
namespace USO.UI.Navigation
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class NavigationBuilder
    {

        public NavigationBuilder Add(string caption, string position, Action<NavigationItemBuilder> itemBuilder)
        {
            var childBuilder = new NavigationItemBuilder();

            childBuilder.Caption(caption);
            childBuilder.Position(position);
            itemBuilder(childBuilder);
            return this;
        }

        public NavigationBuilder Add(string caption, Action<NavigationItemBuilder> itemBuilder)
        {
            return Add(caption, null, itemBuilder);
        }
        public NavigationBuilder Add(Action<NavigationItemBuilder> itemBuilder)
        {
            return Add(null, null, itemBuilder);
        }
        public NavigationBuilder Add(string caption, string position)
        {
            return Add(caption, position, x => { });
        }
        public NavigationBuilder Add(string caption)
        {
            return Add(caption, null, x => { });
        }

    }
}