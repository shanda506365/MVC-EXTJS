
namespace USO.Core.Services
{
    using System.IO;

    public interface IHashGetter : IDependency
    {
        ComputedHash GetHashFromFile(Stream stream);
    }
}
