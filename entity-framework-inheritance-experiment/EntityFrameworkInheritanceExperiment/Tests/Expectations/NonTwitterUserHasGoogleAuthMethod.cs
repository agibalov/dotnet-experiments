using System.Linq;
using EntityFrameworkInheritanceExperiment.DTO;
using EntityFrameworkInheritanceExperiment.Service;
using NUnit.Framework;

namespace EntityFrameworkInheritanceExperiment.Tests.Expectations
{
    public class NonTwitterUserHasGoogleAuthMethod : IExpectation
    {
        public void Check(AuthenticationService authenticationService)
        {
            var allUsers = authenticationService.GetAllUsers();
            var user = allUsers.Single(u => !u.AuthenticationMethods.Any(authMethod => authMethod is TwitterAuthenticationMethodDTO));
            Assert.That(user.AuthenticationMethods.OfType<GoogleAuthenticationMethodDTO>().Count(), Is.EqualTo(1));
        }
    }
}