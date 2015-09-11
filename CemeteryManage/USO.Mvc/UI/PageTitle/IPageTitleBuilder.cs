
namespace USO.UI.PageTitle
{
    using USO.Core;

    public interface IPageTitleBuilder : IDependency
    {
        void AddTitleParts(params string[] titleParts);
        void AppendTitleParts(params string[] titleParts);
        string GenerateTitle();
    }
}