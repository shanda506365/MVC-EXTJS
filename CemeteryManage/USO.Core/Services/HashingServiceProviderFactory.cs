
namespace USO.Core.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;
    using USO.Core.Enums;

    public class HashingServiceProviderFactory : IHashingServiceProviderFactory
    {
        private readonly IConfigSettings _configSettings;
        private readonly IEnumerable<IHashingServiceProvider> _providers;

        public HashingServiceProviderFactory(IConfigSettings configSettings, IDependencyResolver dependencyResolver)
        {
            _configSettings = configSettings;
            _providers = dependencyResolver.GetServices<IHashingServiceProvider>();
        }

        public IHashingServiceProvider GetProvider()
        {
            var hashingAlgorithmToUse = HashingAlgorithm.FromName(_configSettings.HashAlgorithm);

            var instance =
                _providers.Where(provider => hashingAlgorithmToUse == provider.HashingAlgorithm).FirstOrDefault();

            if (instance == null)
            {
                throw new Exception(string.Format("Could not find implementation for the HashingAlgorithm {0}.",
                                                  hashingAlgorithmToUse));
            }
            return instance;
        }
    }
}
