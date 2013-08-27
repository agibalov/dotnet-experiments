using System.Collections.Generic;
using EntityFrameworkInheritanceExperiment.DTO;
using EntityFrameworkInheritanceExperiment.Service;

namespace EntityFrameworkInheritanceExperiment.Tests.AuthenticationSteps
{
    public class AuthenticateWithGoogleAuthenticationStep : AuthenticationStep
    {
        private readonly string _googleUserId;
        private readonly string _email;

        public AuthenticateWithGoogleAuthenticationStep(string googleUserId, string email)
        {
            _googleUserId = googleUserId;
            _email = email;
        }

        public override UserDTO Run(AuthenticationService authenticationService, IList<IContextRequirement> contextRequirements)
        {
            return authenticationService.AuthenticateWithGoogle(_googleUserId, _email);
        }

        public override string ToString()
        {
            return "Google";
        }
    }
}