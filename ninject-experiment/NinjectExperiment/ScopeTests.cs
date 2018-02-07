using NUnit.Framework;
using Ninject;

namespace NinjectExperiment
{
    public class ScopeTests
    {
        [Test]
        public void Test()
        {
            var kernel = new StandardKernel();

            kernel.Bind<ScopedService>().ToSelf().InScope(x => ProcessingScope.Current);
            kernel.Bind<ScopeObject>().ToMethod(x => ProcessingScope.Current);

            var scopeObjectA = new ScopeObject { Name = "A" };
            var scopeObjectB = new ScopeObject { Name = "B" };

            // scope A
            ProcessingScope.Current = scopeObjectA;
            var serviceA1 = kernel.Get<ScopedService>();
            Assert.AreSame(scopeObjectA, serviceA1.Scope);
            
            var serviceA2 = kernel.Get<ScopedService>();
            Assert.AreSame(scopeObjectA, serviceA1.Scope);
            
            Assert.AreSame(serviceA1, serviceA2);

            // scope B
            ProcessingScope.Current = scopeObjectB;
            var serviceB1 = kernel.Get<ScopedService>();
            Assert.AreSame(scopeObjectB, serviceB1.Scope);
            Assert.AreNotSame(serviceA1, serviceB1);
        }

        public class ScopeObject
        {
            public string Name { get; set; }

            public override string ToString()
            {
                return string.Format("Scope{{{0}}}", Name);
            }
        }

        public class ProcessingScope
        {
            public static ScopeObject Current { get; set; }
        }

        public class ScopedService
        {
            [Inject]
            public ScopeObject Scope { get; set; }
        }
    }
}