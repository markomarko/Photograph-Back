using System;
using System.Collections.Generic;
using System.Web.Http.Dependencies;
using Ninject;
using Ninject.Modules;
using Photograph.BLL.Shared;

namespace Photograph.WebApi
{
    internal class NinjectResolver : IDependencyResolver
    {
        private static IKernel _kernel;

        internal static IKernel BuildKernel()
        {
            if (_kernel != null) return _kernel;

            var modules = new INinjectModule[]
            {
                new BLLNinjectModule(),
                new WebApiNinjectModule()
            };

            _kernel = new StandardKernel(modules);

            return _kernel;
        }

        public object GetService(Type serviceType)
        {
            return _kernel.TryGet(serviceType);
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            try
            {
                return _kernel.GetAll(serviceType);
            }
            catch (Exception)
            {
                return new List<object>();
            }
        }

        public IDependencyScope BeginScope()
        {
            throw new NotImplementedException();
        }

        public T Resolve<T>(string name = null)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            _kernel.Dispose();
        }
    }
}