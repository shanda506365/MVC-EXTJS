
namespace USO.Core.Services
{
    using System;


    public interface IDateTime : IDependency
    {
        DateTime UtcNow { get; }
    }
}
