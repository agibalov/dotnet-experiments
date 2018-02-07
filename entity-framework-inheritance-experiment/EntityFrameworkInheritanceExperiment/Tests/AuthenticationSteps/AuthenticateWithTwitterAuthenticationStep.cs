using EntityFrameworkInheritanceExperiment.DTO;
using EntityFrameworkInheritanceExperiment.Service;

namespace EntityFrameworkInheritanceExperiment.Tests.AuthenticationSteps
{
    public class AuthenticateWithTwitterAuthenticationStep : AuthenticationStep
    {
        private readonly string _twitterUserId;
        private readonly string _userDisplayName;

        public AuthenticateWithTwitterAuthenticationStep(string twitterUserId, string userDisplayName)
        {
            _twitterUserId = twitterUserId;
            _userDisplayName = userDisplayName;
        }

        public override UserDTO Run(AuthenticationService authenticationService)
        {
            return authenticationService.AuthenticateWithTwitter(_twitterUserId, _userDisplayName);
        }

        public override string ToString()
        {
            return "Twitter";
        }
    }
}