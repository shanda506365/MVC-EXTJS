
namespace USO.Core.Services
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Text;
    using USO.Core.Enums;

    public class HashGetter : IHashGetter
    {
        private readonly IHashingServiceProvider _hashingServiceProvider;
        private readonly HashEncodingType _hashEncodingType;

        public HashGetter(IHashingServiceProviderFactory hashingServiceProviderFactory, IHashEncodingTypeFactory hashEncodingTypeFactory)
        {
            if (hashEncodingTypeFactory == null)
            {
                throw new ArgumentNullException("hashEncodingTypeFactory");
            }
            _hashingServiceProvider = hashingServiceProviderFactory.GetProvider();
            _hashEncodingType = hashEncodingTypeFactory.GetEncodingType();
        }

        public ComputedHash GetHashFromFile(Stream stream)
        {
            byte[] computedHash = _hashingServiceProvider.ComputeHash(stream);
            string hashString = GetHashString(computedHash);
            return new ComputedHash(hashString, _hashingServiceProvider.HashingAlgorithm);
        }

        private string GetHashString(byte[] computedHash)
        {
            string hashString;
            if (_hashEncodingType == HashEncodingType.Hex)
            {
                hashString = GetHexHashString(computedHash);
            }
            else if (_hashEncodingType == HashEncodingType.Base64)
            {
                hashString = Convert.ToBase64String(computedHash);
            }
            else
            {
                throw new NotSupportedException(string.Format("The HashEncodingType {0} is not supported.", _hashEncodingType));
            }
            return hashString;
        }

        private static string GetHexHashString(IEnumerable<byte> computedHash)
        {
            var hashStringBuilder = new StringBuilder();
            foreach (var @byte in computedHash)
            {
                hashStringBuilder.Append(@byte.ToString("x2"));
            }
            return hashStringBuilder.ToString();
        }
    }
}
