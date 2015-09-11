﻿
namespace USO.Core.FileSystems.VirtualPath
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using USO.Core.Caching;

    public interface IVirtualPathProvider : IVolatileProvider
    {
        string Combine(params string[] paths);
        string ToAppRelative(string virtualPath);
        string MapPath(string virtualPath);

        bool FileExists(string virtualPath);
        Stream OpenFile(string virtualPath);
        StreamWriter CreateText(string virtualPath);
        Stream CreateFile(string virtualPath);
        DateTime GetFileLastWriteTimeUtc(string virtualPath);

        bool DirectoryExists(string virtualPath);
        void CreateDirectory(string virtualPath);
        string GetDirectoryName(string virtualPath);

        IEnumerable<string> ListFiles(string path);
        IEnumerable<string> ListDirectories(string path);
    }
}
