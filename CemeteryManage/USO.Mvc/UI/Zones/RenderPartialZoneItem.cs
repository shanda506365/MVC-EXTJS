
namespace USO.UI.Zones
{
    using System.Web.Mvc;
    using System.Web.Mvc.Html;

    public class RenderPartialZoneItem : ZoneItem {
        public object Model { get; set; }
        public string TemplateName { get; set; }

        public override void Execute<TModel>(HtmlHelper<TModel> html) {
            html.RenderPartial(TemplateName, Model);
        }
    }
}