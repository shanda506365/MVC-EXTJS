using System;
using System.Linq;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using USO.Core.Caching;
using USO.Core.Tasks;
using USO.Domain;


namespace USO.Infrastructure.Bots
{
    public class SyncCacheBot : IBot
    {
        private readonly IDisposable subscription;
      

        public void Dispose()
        {
            subscription.Dispose();
        }
    }
}
