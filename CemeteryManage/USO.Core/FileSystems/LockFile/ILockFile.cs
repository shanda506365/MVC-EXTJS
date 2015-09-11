
namespace USO.Core.FileSystems.LockFile
{
    using System;

    public interface ILockFile : IDisposable
    {
        void Release();
    }
}
