using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using IdentityServer3.Core.Services;
using Ninject;
using Ninject.Modules;
using Photograph.BLL.Shared;

namespace Photograph.IdentityServer.App_Start
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
                new IdentityServerNinjectModule()
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

        public T Resolve<T>(string name = null)
        {
            throw new NotImplementedException();
        }
    }
}