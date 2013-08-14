using System;
using System.Collections.Generic;
using System.Web.Http.Dependencies;
using Castle.Core.Internal;
using Castle.Windsor;

namespace Rikstoto.WebAPI.Container
{
    public class WindsorDependencyScope : IDependencyScope
    {
        private IWindsorContainer _container;
        private WindsorDependencyResolver _resolver;

        private HashSet<object> _resolvedComponents = new HashSet<object>();

        public WindsorDependencyScope(IWindsorContainer container, WindsorDependencyResolver resolver)
        {
            _container = container;
            _resolver = resolver;
        }

        public void Dispose()
        {
            if (_container == null)
                throw new ObjectDisposedException("this", "dependency scope already disposed");

            GC.SuppressFinalize(this);

            _resolvedComponents.ForEach(item => _container.Release(item));
            _container = null;
            _resolver = null;
            _resolvedComponents = null;
        }

        public object GetService(Type serviceType)
        {
            object service = _resolver.GetService(serviceType);

            if (!_resolvedComponents.Contains(service))
                _resolvedComponents.Add(service);

            return service;
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            foreach (var service in _resolver.GetServices(serviceType))
            {
                if (!_resolvedComponents.Contains(service))
                    _resolvedComponents.Add(service);

                yield return service;
            }
        }
    }
}