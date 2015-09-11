
namespace USO.Core.Caching
{
    using System;
    using System.Web.Mvc;

    public class CacheManagerFactory : ICacheManagerFactory
    {
        private readonly IDependencyResolver resolver;
        public CacheManagerFactory(IDependencyResolver resolver)
        {
            this.resolver = resolver;
        }
        public ICacheManager CreateCacheManager(Type component)
        {
            return new DefaultCacheManager(component, resolver.GetService<ICacheHolder>());
        }
    }
}
