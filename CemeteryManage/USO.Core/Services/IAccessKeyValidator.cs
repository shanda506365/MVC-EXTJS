
namespace USO.Core.Services
{
   public interface IAccessKeyValidator : IDependency
    {
        bool IsValidAccessKey(string key);
    }
}
