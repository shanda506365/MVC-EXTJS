
namespace USO.UI.Zones
{
    using System.Web.Mvc;
    using USO.Core;

    public interface IZoneManager : IDependency {
        void Render<TModel>(HtmlHelper<TModel> html, ZoneCollection zones, string zoneName, string partitions, string[] except);
    }
}