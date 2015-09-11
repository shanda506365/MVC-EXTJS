
namespace USO.Core.Caching
{
    using System;

    public interface ICacheManagerFactory : IDependency
    {
        ICacheManager CreateCacheManager(Type component);
    }
}
