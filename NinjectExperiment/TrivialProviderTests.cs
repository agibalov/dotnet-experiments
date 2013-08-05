using NUnit.Framework;
using Ninject;
using Ninject.Activation;

namespace NinjectExperiment
{
    public class TrivialProviderTests
    {
        [Test]
        public void CanUseProvider()
        {
            var kernel = new StandardKernel();
            kernel.Bind<Service>().ToProvider<ServiceProvider>();
            ServiceProvider.CreatedInstance = false;
            kernel.Get<Service>();
            Assert.IsTrue(ServiceProvider.CreatedInstance);
        }

        class Service
        {
        }

        class ServiceProvider : Provider<Service>
        {
            public static bool CreatedInstance { get; set; }

            protected override Service CreateInstance(IContext context)
            {
                CreatedInstance = true;
                return new Service();
            }
        }
        
    }
}