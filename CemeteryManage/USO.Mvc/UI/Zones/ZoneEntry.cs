
namespace USO.UI.Zones
{
    using System.Collections.Generic;

    public class ZoneEntry {
        public ZoneEntry() {
            Items = new List<ZoneItem>();
        }

        public string ZoneName { get; set; }
        public IList<ZoneItem> Items { get; set; }
    }
}