
namespace USO.Core.Services
{
    using System.IO;
    using System.Security.Cryptography;
    using USO.Core.Enums;

    public class Sha1HashingServiceProvider : IHashingServiceProvider
    {
        private readonly SHA1 _provider = SHA1.Create();

        public HashingAlgorithm HashingAlgorithm { get { return HashingAlgorithm.SHA1; } }

        public byte[] ComputeHash(Stream stream)
        {
            return _provider.ComputeHash(stream);
        }
    }
}
