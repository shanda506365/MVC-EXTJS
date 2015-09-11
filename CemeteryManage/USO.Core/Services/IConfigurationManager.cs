
namespace USO.Core.Services
{
    using System.Configuration;

    public interface IConfigurationManager : IDependency
    {
        string GetString(string key);
        string GetString(string key,string defaultValue);
        int GetInt(string key);
        bool GetBool(string key);
        bool GetBool(string key, bool defautlValue);
        ConnectionStringSettings GetConnectionStringSettings(string key);
    }
}
