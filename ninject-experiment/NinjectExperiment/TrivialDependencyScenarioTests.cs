using NUnit.Framework;
using Ninject;

namespace NinjectExperiment
{
    public class TrivialDependencyScenarioTests
    {
        [Test]
        public void CanResolveSimpleDependenciesEvenIfNotExplicitlyDefined()
        {
            var kernel = new StandardKernel();
            var service = kernel.Get<MainService>();
            Assert.IsNotNull(service.ServiceA);
            Assert.AreEqual(typeof(ServiceA), service.ServiceA.GetType());
            Assert.IsNotNull(service.ServiceB);
            Assert.AreEqual(typeof(ServiceB), service.ServiceB.GetType());
        }

        class MainService
        {
            public ServiceA ServiceA { get; private set; }
            public ServiceB ServiceB { get; private set; }

            public MainService(ServiceA serviceA, ServiceB serviceB)
            {
                ServiceA = serviceA;
                ServiceB = serviceB;
            }
        }

        class ServiceA
        {
        }

        class ServiceB
        {
        }
    }
}