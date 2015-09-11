
namespace USO.Core.Services
{
    using System.Collections.Generic;

    public interface IMapper : IDependency
    {
        TDestination Map<TSource, TDestination>(TSource source);
        void Map<TSource, TDestination>(TSource source, TDestination destination);
        IList<TDestination> MapMultiple<TSource, TDestination>(IEnumerable<TSource> sources);
    }
}
