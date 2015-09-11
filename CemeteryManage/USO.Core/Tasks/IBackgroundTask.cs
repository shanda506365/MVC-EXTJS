
namespace USO.Core.Tasks
{
    using USO.Core.Events;

    public interface IBackgroundTask : IEventHandler
    {
        void Sweep();
    }
}
