using System.Linq;
using EntityFrameworkInheritanceExperiment.DTO;
using EntityFrameworkInheritanceExperiment.Service;
using NUnit.Framework;

namespace EntityFrameworkInheritanceExperiment.Tests.Expectations
{
    public class ThereIsOneNonTwitterUserExpectation : IExpectation
    {
        public void Check(AuthenticationService authenticationService)
        {
            var allUsers = authenticationService.GetAllUsers();
            var user = allUsers.SingleOrDefault(u => !u.AuthenticationMethods.Any(authMethod => authMethod is TwitterAuthenticationMethodDTO));
            Assert.That(user, Is.Not.Null);
        }
    }
}