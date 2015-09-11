

namespace USO.Core.FileSystems.VirtualPath
{
    using USO.Core.Caching;

    /// <summary>
    /// Enable monitoring changes over virtual path
    /// </summary>
    public interface IVirtualPathMonitor : IVolatileProvider
    {
        IVolatileToken WhenPathChanges(string virtualPath);
    }
}
