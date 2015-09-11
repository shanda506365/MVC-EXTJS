
namespace USO.UI.PageClass
{
    using USO.Core;

    public interface IPageClassBuilder : IDependency
    {
        void AddClassNames(params object[] classNames);
    }
}