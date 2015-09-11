
namespace USO.Core.Caching
{
    public interface IVolatileToken
    {
        bool IsCurrent { get; }
    }
}
