using NUnit.Framework;

namespace EntityFrameworkInheritanceExperiment
{
    public abstract class AbstractAuthenticationServiceTests
    {
        protected readonly AuthenticationService Service = new AuthenticationService("UsersConnectionString");

        [SetUp]
        public void ResetEverything()
        {
            Service.Reset();
        }
    }
}