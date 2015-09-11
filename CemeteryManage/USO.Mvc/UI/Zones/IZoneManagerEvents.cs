
namespace USO.UI.Zones
{
    using System.Collections.Generic;
    using System.Web.Mvc;
    using USO.Core.Events;


    public interface IZoneManagerEvents : IEventHandler {
        void ZoneRendering(ZoneRenderContext context);
        void ZoneItemRendering(ZoneRenderContext context, ZoneItem item);
        void ZoneItemRendered(ZoneRenderContext context, ZoneItem item);
        void ZoneRendered(ZoneRenderContext context);
    }
    public class ZoneRenderContext {
        public HtmlHelper Html { get; set; }
        public ZoneCollection Zones { get; set; }
        public string ZoneName { get; set; }
        public IEnumerable<ZoneItem> RenderingItems { get; set; }
    }

}