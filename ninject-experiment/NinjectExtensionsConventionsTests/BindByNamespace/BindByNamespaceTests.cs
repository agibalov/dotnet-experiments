using NUnit.Framework;
using Ninject;
using Ninject.Extensions.Conventions;
using NinjectExtensionsConventionsTests.BindByNamespace.Handlers;

namespace NinjectExtensionsConventionsTests.BindByNamespace
{
    public class BindByNamespaceTests
    {
        [Test]
        public void CanBindByNamespace()
        {
            var kernel = new StandardKernel();
            kernel.Bind(x => x
                .FromThisAssembly()
                .SelectAllClasses()
                .InNamespaceOf<HandlerOne>()
                .BindToSelf()
                .Configure(b => b.InSingletonScope()));

            Assert.AreSame(kernel.Get<HandlerOne>(), kernel.Get<HandlerOne>());
            Assert.AreSame(kernel.Get<HandlerTwo>(), kernel.Get<HandlerTwo>());
        }
    }
}
