
namespace USO.Core.Services
{
    using System;
    using USO.Core.Enums;


    public interface IHashEncodingTypeFactory : IDependency
    {
        HashEncodingType GetEncodingType();
    }
}
