
namespace USO.UI.Navigation
{
    using USO.Core;

    public interface INavigationProvider : IDependency
    {
        string MenuName { get; }
        void GetNavigation(NavigationBuilder builder);
    }
}