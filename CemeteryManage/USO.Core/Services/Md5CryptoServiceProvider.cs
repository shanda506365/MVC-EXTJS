
namespace USO.Core.Services
{
    using System.IO;
    using System.Security.Cryptography;
    using USO.Core.Enums;

    public class Md5CryptoServiceProvider : IHashingServiceProvider
    {
        private readonly MD5CryptoServiceProvider _provider = new MD5CryptoServiceProvider();

        public HashingAlgorithm HashingAlgorithm { get { return HashingAlgorithm.MD5; } }

        public byte[] ComputeHash(Stream stream)
        {
            return _provider.ComputeHash(stream);
        }
    }
}
