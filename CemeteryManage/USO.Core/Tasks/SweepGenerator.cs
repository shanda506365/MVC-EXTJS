
namespace USO.Core.Tasks
{
    using System;
    using System.Web.Mvc;
    using System.Timers;
    using USO.Core.Logging;
    using MvcExtensions;

    public class SweepGenerator : BootstrapperTask
    {
        private readonly Timer _timer;
        private readonly ILogger _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="RegisterModelMetadata"/> class.
        /// </summary>
        /// <param name="container">The container.</param>
        public SweepGenerator(ContainerAdapter container)
        {
            Invariant.IsNotNull(container, "container");
            Container = container;

            _timer = new Timer();
            _timer.Elapsed += Elapsed;            
            Interval = TimeSpan.FromMinutes(1);           
            _logger = container.GetService<ILoggerFactory>().CreateLogger(GetType());
        }

        /// <summary>
        /// Gets the container.
        /// </summary>
        /// <value>The container.</value>
        protected ContainerAdapter Container
        {
            get;
            private set;
        }

        public TimeSpan Interval
        {
            get { return TimeSpan.FromMilliseconds(_timer.Interval); }
            set { _timer.Interval = value.TotalMilliseconds; }
        }

        /// <summary>
        /// Executes the task. Returns continuation of the next task(s) in the chain.
        /// </summary>
        /// <returns></returns>
        public override TaskContinuation Execute()
        {
            lock (_timer)
            {
                _timer.Start();
            }

            return TaskContinuation.Continue;
        }

        protected override void DisposeCore()
        {
            lock (_timer)
            {
                _timer.Stop();
            }
        }

        void Elapsed(object sender, ElapsedEventArgs e)
        {
            // current implementation disallows re-entrancy
            if (!System.Threading.Monitor.TryEnter(_timer))
                return;

            try
            {
                if (_timer.Enabled)
                {
                    DoWork();
                }
            }
            catch (Exception ex)
            {
                _logger.Warn(ex, "Problem in background tasks");
            }
            finally
            {
                System.Threading.Monitor.Exit(_timer);
            }
        }

        public void DoWork()
        {
            try
            {
                // resolve the manager and invoke it
                var manager = Container.GetService<IBackgroundService>();
                manager.Sweep();
            }
            catch
            {
                // pass exception along to actual handler
                throw;
            }
        }

    }
}
