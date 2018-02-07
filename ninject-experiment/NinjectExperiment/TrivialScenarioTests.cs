using NUnit.Framework;
using Ninject;

namespace NinjectExperiment
{
    public class TrivialScenarioTests
    {
        [Test]
        public void CanGetServiceByConcreteTypeEvenIfNotBoundExplicitly()
        {
            var kernel = new StandardKernel();
            var service = kernel.Get<Service>();
            Assert.AreEqual(typeof(Service), service.GetType());
        }

        [Test]
        public void CantGetServiceByInterfaceIfNotBoundExplicitly()
        {
            var kernel = new StandardKernel();
            var service = kernel.TryGet<IService>();
            Assert.IsNull(service);
        }

        [Test]
        public void CanGetServiceByInterfaceIfBoundExplicitly()
        {
            var kernel = new StandardKernel();
            kernel.Bind<IService>().To<Service>();
            var service = kernel.Get<IService>();
            Assert.AreEqual(typeof(Service), service.GetType());
        }

        [Test]
        public void ServiceIsNotSingletonUnlessExplicitlyConfigured()
        {
            var kernel = new StandardKernel();
            var instance1 = kernel.Get<Service>();
            var instance2 = kernel.Get<Service>();
            Assert.AreNotEqual(instance1, instance2);
        }

        [Test]
        public void ServiceIsSingletonWhenExplicitlyConfigured()
        {
            var kernel = new StandardKernel();
            kernel.Bind<Service>().ToSelf().InSingletonScope();
            var instance1 = kernel.Get<Service>();
            var instance2 = kernel.Get<Service>();
            Assert.AreEqual(instance1, instance2);
        }

        interface IService
        {
        }

        class Service : IService
        {
        }
    }
}