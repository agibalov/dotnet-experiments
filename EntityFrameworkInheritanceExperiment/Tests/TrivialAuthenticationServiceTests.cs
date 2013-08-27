using NUnit.Framework;

namespace EntityFrameworkInheritanceExperiment.Tests
{
    public class TrivialAuthenticationServiceTests : AbstractAuthenticationServiceTests
    {
        [Test]
        public void ThereAreNoUsersByDefault()
        {
            Assert.That(Service.GetUserCount(), Is.EqualTo(0));
        }
    }
}