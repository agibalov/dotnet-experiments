using NUnit.Framework;
using Ninject;
using Ninject.Extensions.Conventions;

namespace NinjectExtensionsConventionsTests.BindByAttribute
{
    public class BindByAttributeTests
    {
        [Test]
        public void CanBindByAttribute()
        {
            var kernel = new StandardKernel();
            kernel.Bind(x => x
                .FromThisAssembly()
                .SelectAllClasses()
                .WithAttribute<HandlerAttribute>()
                .BindToSelf()
                .Configure(b => b.InSingletonScope()));

            Assert.AreSame(kernel.Get<HandlerOne>(), kernel.Get<HandlerOne>());
            Assert.AreSame(kernel.Get<HandlerTwo>(), kernel.Get<HandlerTwo>());
        }
    }
}