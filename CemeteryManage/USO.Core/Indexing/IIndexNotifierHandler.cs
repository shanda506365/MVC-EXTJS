

namespace USO.Core.Indexing
{
    using USO.Core.Events;

    public interface IIndexNotifierHandler : IEventHandler
    {
        void UpdateIndex(string indexName);
    }
}
