
namespace USO.Core.Services
{
    public interface IHashingServiceProviderFactory : IDependency
    {
        IHashingServiceProvider GetProvider();
    }
}
