namespace USO.Infrastructure
{
    using System;
    using System.Linq;
    using System.Collections.Generic;
    using System.Collections.Concurrent;
    using System.Web.Mvc;
    using MvcExtensions;

    public abstract class WorkContext
    {
        public abstract T Resolve<T>();

        public abstract T GetState<T>(string name);

        public abstract void SetState<T>(string name, T value);
        
    }

    public class WorkContextImplementation : WorkContext
    {
        readonly ConcurrentDictionary<string, Func<object>> _stateResolvers = new ConcurrentDictionary<string, Func<object>>();
        readonly IEnumerable<IWorkContextStateProvider> _workContextStateProviders;
        readonly ContainerAdapter _containerAdapter;
        
        public WorkContextImplementation(ContainerAdapter containerAdapter)
        {
            _containerAdapter = containerAdapter;
            _workContextStateProviders = containerAdapter.GetServices<IWorkContextStateProvider>();

            //Settings = Resolve<Settings>();
        }

        public override T Resolve<T>()
        {
            return DependencyResolver.Current.GetService<T>();
        }


        public override T GetState<T>(string name)
        {
            var resolver = _stateResolvers.GetOrAdd(name, FindResolverForState<T>);
            return (T)resolver();
        }

        Func<object> FindResolverForState<T>(string name)
        {
            var resolver = _workContextStateProviders.Select(wcsp => wcsp.Get<T>(name))
                .FirstOrDefault(value => !Equals(value, default(T)));

            if (resolver == null)
            {
                return () => default(T);
            }
            return () => resolver();
        }


        public override void SetState<T>(string name, T value)
        {
            _stateResolvers[name] = () => value;
        }
    }

    public class WorkContextImplementationForStore : WorkContext
    {
        readonly ConcurrentDictionary<string, Func<object>> _stateResolvers = new ConcurrentDictionary<string, Func<object>>();
        readonly IEnumerable<IWorkContextStateProvider> _workContextStateProviders;
        readonly ContainerAdapter _containerAdapter;

        public WorkContextImplementationForStore(ContainerAdapter containerAdapter)
        {
            
        }

        public override T Resolve<T>()
        {
            return DependencyResolver.Current.GetService<T>();
        }


        public override T GetState<T>(string name)
        {
            var resolver = _stateResolvers.GetOrAdd(name, FindResolverForState<T>);
            return (T)resolver();
        }

        Func<object> FindResolverForState<T>(string name)
        {
            //var resolver = _workContextStateProviders.Select(wcsp => wcsp.Get<T>(name))
            //    .FirstOrDefault(value => !Equals(value, default(T)));

            //if (resolver == null)
            //{
            //    return () => default(T);
            //}
            //return () => resolver();
            return () => null;
        }


        public override void SetState<T>(string name, T value)
        {
            return;
            //_stateResolvers[name] = () => value;
        }
    }
}
