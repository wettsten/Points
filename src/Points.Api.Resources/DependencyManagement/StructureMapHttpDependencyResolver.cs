using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http.Dependencies;
using StructureMap;

namespace Points.Api.Resources.DependencyManagement
{
    public class StructureMapHttpDependencyResolver : IDependencyResolver
    {
        readonly IContainer _container;

        public StructureMapHttpDependencyResolver(IContainer container)
        {
            _container = container;
        }

        public IDependencyScope BeginScope()
        {
            return this;
        }

        public object GetService(Type serviceType)
        {
            if (serviceType.IsClass)
            {
                return GetConcreteService(serviceType);
            }
            else
            {
                return GetInterfaceService(serviceType);
            }
        }

        private object GetConcreteService(Type serviceType)
        {
            try
            {
                // Can't use TryGetInstance here because it won’t create concrete types
                return _container.GetInstance(serviceType);
            }
            catch (StructureMapException)
            {
                return null;
            }
        }

        private object GetInterfaceService(Type serviceType)
        {
            return _container.TryGetInstance(serviceType);
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            return _container.GetAllInstances(serviceType).Cast<object>();
        }

        public void Dispose()
        {
        }
    }
}