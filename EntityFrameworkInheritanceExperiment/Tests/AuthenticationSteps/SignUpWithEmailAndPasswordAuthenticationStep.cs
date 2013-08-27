using System.Collections.Generic;
using EntityFrameworkInheritanceExperiment.DTO;
using EntityFrameworkInheritanceExperiment.Service;
using NUnit.Framework;

namespace EntityFrameworkInheritanceExperiment.Tests.AuthenticationSteps
{
    public class SignUpWithEmailAndPasswordAuthenticationStep : AuthenticationStep
    {
        private readonly string _email;
        private readonly string _password;

        public SignUpWithEmailAndPasswordAuthenticationStep(string email, string password)
        {
            _email = email;
            _password = password;
        }

        public override UserDTO Run(AuthenticationService authenticationService, IList<IContextRequirement> contextRequirements)
        {
            var user = authenticationService.SignUpWithEmailAndPassword(_email, _password);
            Assert.That(user.UserId, Is.GreaterThan(0));
            // todo

            contextRequirements.Add(new ThereShouldBeUserWithId(user.UserId));

            return user;
        }

        public override string ToString()
        {
            return "Password";
        }
    }
}