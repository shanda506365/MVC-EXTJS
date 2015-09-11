
namespace USO.Core.Tasks
{
    using System.Web.Mvc;
    using USO.Core.Logging;
    using System.Collections.Generic;

    public interface IBackgroundService : IDependency
    {
        void Sweep();
    }

    public class BackgroundService : IBackgroundService
    {
        private readonly IEnumerable<IBackgroundTask> _tasks;
        private readonly ILogger _logger;

        public BackgroundService(IDependencyResolver dependencyResolver, ILoggerFactory loggerFactory)
        {
            _tasks = dependencyResolver.GetServices<IBackgroundTask>();
            _logger = loggerFactory.CreateLogger(GetType());
        }

        public void Sweep()
        {
            _tasks.Invoke(task => task.Sweep(), _logger);
        }
    }
}
