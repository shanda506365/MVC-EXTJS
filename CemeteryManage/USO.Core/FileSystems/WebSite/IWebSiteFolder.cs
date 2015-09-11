﻿
namespace USO.Core.FileSystems.WebSite
{
    using System.Collections.Generic;
    using System.IO;
    using USO.Core.Caching;

    /// <summary>
    /// Abstraction over the virtual files/directories of a web site.
    /// </summary>
    public interface IWebSiteFolder : IVolatileProvider
    {
        IEnumerable<string> ListDirectories(string virtualPath);
        IEnumerable<string> ListFiles(string virtualPath, bool recursive);

        bool FileExists(string virtualPath);
        string ReadFile(string virtualPath);
        string ReadFile(string virtualPath, bool actualContent);
        void CopyFileTo(string virtualPath, Stream destination);
        void CopyFileTo(string virtualPath, Stream destination, bool actualContent);

        IVolatileToken WhenPathChanges(string virtualPath);
    }
}
