using System.Linq;
using NUnit.Framework;
using Ninject;

namespace NinjectExperiment
{
    public class MultipleBindingTests
    {
        [Test]
        public void CanHaveMultipleServicesWithSameInterface()
        {
            var kernel = new StandardKernel();
            kernel.Bind<IService>().To<ServiceA>();
            kernel.Bind<IService>().To<ServiceB>();
            kernel.Bind<IService>().To<ServiceC>();
            var services = kernel.GetAll<IService>().ToList();
            Assert.IsTrue(services.Any(service => service.GetType() == typeof(ServiceA)));
            Assert.IsTrue(services.Any(service => service.GetType() == typeof(ServiceB)));
            Assert.IsTrue(services.Any(service => service.GetType() == typeof(ServiceC)));
        }

        interface IService
        {
        }

        class ServiceA : IService
        {
        }

        class ServiceB : IService
        {
        }

        class ServiceC : IService
        {
        }
    }
}