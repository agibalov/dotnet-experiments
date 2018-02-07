using System;
using Ninject;
using NUnit.Framework;

namespace NinjectExperiment
{
    public class SelectiveInjectionTests
    {
        [Test]
        public void ItShouldBePossibleToMakeTheDependencyAvailableOnlyToASingleService()
        {
            var kernel = new StandardKernel();
            kernel.Bind<TheDependencyWhichIWantToOnlyBeAvailableForServiceA>()
                .ToMethod(c =>
                {
                    throw new Exception("YOU CAN'T GET IT");
                });
            kernel.Bind<TheDependencyWhichIWantToOnlyBeAvailableForServiceA>()
                .ToSelf()
                .WhenInjectedExactlyInto<ServiceA>()
                .InSingletonScope();

            Assert.NotNull(kernel.Get<ServiceA>().TheDependencyWhichIWantToOnlyBeAvailableForServiceA);

            try
            {
                kernel.Get<ServiceB>();
                Assert.Fail();
            }
            catch (Exception e)
            {
                Assert.AreEqual("YOU CAN'T GET IT", e.Message);
            }
        }

        public class TheDependencyWhichIWantToOnlyBeAvailableForServiceA
        {
        }

        public class ServiceA
        {
            [Inject]
            public TheDependencyWhichIWantToOnlyBeAvailableForServiceA TheDependencyWhichIWantToOnlyBeAvailableForServiceA { get; set; }
        }

        public class ServiceB
        {
            [Inject]
            public TheDependencyWhichIWantToOnlyBeAvailableForServiceA TheDependencyWhichIWantToOnlyBeAvailableForServiceA { get; set; }
        }
    }
}