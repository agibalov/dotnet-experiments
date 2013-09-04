using EntityFrameworkInheritanceExperiment.DAL;
using EntityFrameworkInheritanceExperiment.Service;
using NUnit.Framework;
using Ninject;

namespace EntityFrameworkInheritanceExperiment.Tests
{
    public abstract class AbstractAuthenticationServiceTests
    {
        private IKernel _kernel;
        protected AuthenticationService Service;

        [SetUp]
        public void ResetEverything()
        {
            _kernel = new StandardKernel();

            _kernel.Bind<UserContext>()
                .ToMethod(x => new UserContext("UsersConnectionString"))
                .InSingletonScope();
            
            _kernel.Bind<string>()
                .ToConstant("UsersConnectionString")
                .Named("ConnectionStringName");

            Service = _kernel.Get<AuthenticationService>();
            Service.Reset();
            Service = _kernel.Get<AuthenticationService>(); // TODO: get rid of dirty hack
        }

        [TearDown]
        public void CleanUp()
        {
            _kernel.Dispose();
        }
    }
}