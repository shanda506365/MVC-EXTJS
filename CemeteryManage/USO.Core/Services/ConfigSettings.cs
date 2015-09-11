
namespace USO.Core.Services
{
    using System;
    using USO.Core.Exceptions;
    using System.Configuration;

    public class ConfigSettings : IConfigSettings
    {
        private readonly IHttpRuntime _httpRuntime;
        private readonly IConfigurationManager _configurationManager;

        public ConfigSettings(IHttpRuntime httpRuntime, IConfigurationManager configurationManager)
        {
            _httpRuntime = httpRuntime;
            _configurationManager = configurationManager;
        }

        public string TryGetString(string key)
        {
            try
            {
                return _configurationManager.GetString(key);
            }
            catch (MissingAppSettingException)
            {
                return string.Empty;
            }
        }

        public ConnectionStringSettings ConnectionStringSettings(string key)
        {
            return _configurationManager.GetConnectionStringSettings(key);
        }

        public string PhysicalSitePath { get { return _httpRuntime.AppDomainAppPath; } }
        public string RelativeTemporaryDirectory { get { return _configurationManager.GetString("RelativeTemporaryDirectory"); } }
        public string ResourceRoot { get { return _configurationManager.GetString("ResourceRoot"); } }
        public string HashAlgorithm { get { return _configurationManager.GetString("HashAlgorithm"); } }
        public string HashEncodingType { get { return _configurationManager.GetString("HashEncodingType"); } }
        public string MigratorProvider { get { return _configurationManager.GetString("MigratorProvider"); } }
        public string RelativeAssemblyDirectory { get { return _configurationManager.GetString("RelativeAssemblyDirectory"); } }
        public bool ExecuteTablePartition { get { return _configurationManager.GetBool("ExecuteTablePartition"); } }
        public bool CustomerInProcessing { get { return _configurationManager.GetBool("CustomerInProcessing",false); } }
    }
}
