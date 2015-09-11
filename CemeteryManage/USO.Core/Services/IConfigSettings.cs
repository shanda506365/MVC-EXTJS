
namespace USO.Core.Services
{
    using System;
    using System.Configuration;


    public interface IConfigSettings : IDependency
    {
        string TryGetString(string key);
        ConnectionStringSettings ConnectionStringSettings(string key);

        string PhysicalSitePath { get; }
        string RelativeTemporaryDirectory { get; }
        string ResourceRoot { get; }
        string HashAlgorithm { get; }
        string HashEncodingType { get; }
        string MigratorProvider { get; }
        string RelativeAssemblyDirectory { get; }
        bool ExecuteTablePartition { get; }
        bool CustomerInProcessing { get; }
    }
}
