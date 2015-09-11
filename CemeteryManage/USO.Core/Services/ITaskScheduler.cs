
namespace USO.Core.Services
{
    using System;

    public interface ITaskScheduler : IDependency
    {
        void ScheduleTask(Action actionToInvoke);
    }
}
