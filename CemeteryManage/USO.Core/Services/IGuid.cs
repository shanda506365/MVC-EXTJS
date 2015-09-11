
namespace USO.Core.Services
{
    using System;

    public interface IGuid : IDependency
    {
        Guid NewGuid();
    }
}
