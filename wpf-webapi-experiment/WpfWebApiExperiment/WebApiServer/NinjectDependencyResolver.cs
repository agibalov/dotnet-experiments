using System;
using System.Collections.Generic;
using System.Web.Http.Dependencies;
using Ninject;
using Ninject.Extensions.ChildKernel;

namespace WpfWebApiExperiment.WebApiServer
{
    public class NinjectDependencyResolver : IDependencyResolver
    {
        private readonly IKernel _kernel;

        public NinjectDependencyResolver(IKernel kernel)
        {
            _kernel = kernel;
        }

        public IDependencyScope BeginScope()
        {
            var scopeKernel = new ChildKernel(_kernel);
            return new NinjectDependencyScope(scopeKernel);
        }

        public object GetService(Type serviceType)
        {
            return _kernel.TryGet(serviceType);
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            return _kernel.GetAll(serviceType);
        }

        public void Dispose()
        {
            _kernel.Dispose();
        }
    }
}