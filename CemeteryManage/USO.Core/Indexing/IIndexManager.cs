namespace USO.Core.Indexing
{
    public interface IIndexManager : IDependency
    {

        bool HasIndexProvider();
        IIndexProvider GetSearchIndexProvider();
    }
}
