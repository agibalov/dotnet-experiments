using NUnit.Framework;
using Ninject;

namespace NinjectExperiment
{
    public class ConfigurationTests
    {
        [Test]
        public void Test()
        {
            var kernel = new StandardKernel();
            kernel.Bind<Configuration>().ToConstant(new Configuration
                {
                    Period = 123
                });

            kernel.Bind<int>().ToMethod(x => x.Kernel.Get<Configuration>().Period).Named("period");

            var service = kernel.Get<Service>();
            Assert.AreEqual(123, service.Period);
        }

        class Configuration
        {
            [Named("period")]
            public int Period { get; set; }
        }

        class Service
        {
            public int Period { get; private set; }

            // option: public Service([Named("period")] int thePeriod)
            public Service(/*[Named("period")]*/ int period)
            {
                Period = period;
            }
        }
    }
}