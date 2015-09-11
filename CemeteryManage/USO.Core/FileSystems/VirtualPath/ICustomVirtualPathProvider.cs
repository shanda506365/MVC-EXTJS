
namespace USO.Core.FileSystems.VirtualPath
{
    using System.Web.Hosting;

    public interface ICustomVirtualPathProvider
    {
        VirtualPathProvider Instance { get; }
    }
}
