using System;
using System.Collections.Generic;
using System.Web.Http.Dependencies;
using Ninject;

namespace WpfWebApiExperiment.WebApiServer
{
    public class NinjectDependencyScope : IDependencyScope
    {
        private readonly IKernel _kernel;

        public NinjectDependencyScope(IKernel kernel)
        {
            _kernel = kernel;
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