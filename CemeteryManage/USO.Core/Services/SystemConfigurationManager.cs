
namespace USO.Core.Services
{
    using System.Configuration;
    using USO.Core.Exceptions;

    public class SystemConfigurationManager : IConfigurationManager
    {
        public string GetString(string key)
        {
            string setting = ConfigurationManager.AppSettings[key];
            if (setting == null)
            {
                throw new MissingAppSettingException(key);
            }
            return setting;
        }

        public string GetString(string key, string defaultValue)
        {
            string setting = ConfigurationManager.AppSettings[key];
            if (setting == null)
            {
                return defaultValue;
            }
            return setting;
        }

        public int GetInt(string key)
        {
            int intProperty;
            string intPropertyAsString = GetString(key);
            if (!int.TryParse(intPropertyAsString, out intProperty))
            {
                throw new InvalidAppSettingException(key, intPropertyAsString);
            }
            return intProperty;
        }

        public bool GetBool(string key)
        {
            bool boolProperty;
            string boolPropertyAsString = GetString(key);
            if (!bool.TryParse(boolPropertyAsString, out boolProperty))
            {
                throw new InvalidAppSettingException(key, boolPropertyAsString);
            }
            return boolProperty;
        }
        public bool GetBool(string key,bool defaultValue)
        {
            bool boolProperty;
            string boolPropertyAsString = GetString(key, defaultValue.ToString());
            if (!bool.TryParse(boolPropertyAsString, out boolProperty))
            {
                return defaultValue;
            }
            return boolProperty;
        }

        public ConnectionStringSettings GetConnectionStringSettings(string key)
        {
            var settings = ConfigurationManager.ConnectionStrings[key];
            if (settings == null)
            {
                throw new MissingConnectionStringSettingsException(key);
            }
            return settings;
        }
    }
}
