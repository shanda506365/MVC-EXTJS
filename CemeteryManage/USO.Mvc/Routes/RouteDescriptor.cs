
namespace USO.Mvc.Routes
{
    using System.Web.Routing;

    public class RouteDescriptor
    {
        public string Name { get; set; }
        public int Priority { get; set; }
        public RouteBase Route { get; set; }
    }
}