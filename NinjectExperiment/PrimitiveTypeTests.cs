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

    public class PrimitiveTypeTests
    {
        [Test]
        public void CanInjectPrimitiveValuesUsingExplicitConstructorArgumentBindings()
        {
            var kernel = new StandardKernel();
            kernel.Bind<Service>().ToSelf().InSingletonScope().WithConstructorArgument("period", 123);
            var service = kernel.Get<Service>();
            Assert.AreEqual(123, service.Period);
        }

        [Test]
        public void CanInjectPrimitveValuesUsingNamedConstructorArguments()
        {
            var kernel = new StandardKernel();
            kernel.Bind<int>().ToConstant(123).Named("period");
            var service = kernel.Get<Service2>();
            Assert.AreEqual(123, service.Period);
        }

        class Service
        {
            public int Period { get; private set; }

            public Service(int period)
            {
                Period = period;
            }
        }

        class Service2
        {
            public int Period { get; private set; }

            public Service2([Named("period")] int period)
            {
                Period = period;
            }
        }
    }
}