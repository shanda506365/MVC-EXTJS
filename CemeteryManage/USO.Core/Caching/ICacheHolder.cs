
namespace USO.Core.Caching
{
    using System;


    public interface ICacheHolder : ISingletonDependency
    {
        ICache<TKey, TResult> GetCache<TKey, TResult>(Type component);
    }
}
