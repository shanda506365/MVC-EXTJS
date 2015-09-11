
namespace USO.Core.Services
{
    using System;
    using USO.Core.Enums;

    public class HashEncodingTypeFactory : IHashEncodingTypeFactory
    {
        private readonly IConfigSettings _configSettings;

        public HashEncodingTypeFactory(IConfigSettings configSettings)
        {
            _configSettings = configSettings;
        }

        public HashEncodingType GetEncodingType()
        {
            return HashEncodingType.FromName(_configSettings.HashEncodingType);
        }
    }
}
