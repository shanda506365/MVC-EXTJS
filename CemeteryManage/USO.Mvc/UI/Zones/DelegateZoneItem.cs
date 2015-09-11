
namespace USO.UI.Zones 
{
    using System;
    using System.Web.Mvc;

    public class DelegateZoneItem : ZoneItem {
        public Action<HtmlHelper> Action { get; set; }

        public override void Execute<TModel>(HtmlHelper<TModel> html) {
            Action(html);
        }
    }
}