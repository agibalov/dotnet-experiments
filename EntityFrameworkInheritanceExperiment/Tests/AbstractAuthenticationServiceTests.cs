using EntityFrameworkInheritanceExperiment.Service;
using EntityFrameworkInheritanceExperiment.Service.Configuration;
using NUnit.Framework;
using Ninject;
using Ninject.Extensions.Conventions;

namespace EntityFrameworkInheritanceExperiment.Tests
{
    public abstract class AbstractAuthenticationServiceTests
    {
        protected AuthenticationService Service;

        [SetUp]
        public void ResetEverything()
        {
            var kernel = new StandardKernel();

            kernel.Bind(x => x
                .FromThisAssembly()
                .SelectAllClasses()
                .WithAttribute<ServiceAttribute>()
                .BindToSelf()
                .Configure(b => b.InSingletonScope()));

            kernel.Bind<string>()
                .ToConstant("UsersConnectionString")
                .Named("ConnectionStringName");

            Service = kernel.Get<AuthenticationService>();

            Service.Reset();
        }
    }
}