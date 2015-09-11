
namespace USO.Core.Services
{
    using System.IO;
    using USO.Core.Enums;

    public interface IHashingServiceProvider : IDependency
    {
        HashingAlgorithm HashingAlgorithm { get; }
        byte[] ComputeHash(Stream stream);
    }
}
