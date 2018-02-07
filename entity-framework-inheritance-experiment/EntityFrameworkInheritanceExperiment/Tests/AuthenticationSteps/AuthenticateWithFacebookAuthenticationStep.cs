using EntityFrameworkInheritanceExperiment.DTO;
using EntityFrameworkInheritanceExperiment.Service;

namespace EntityFrameworkInheritanceExperiment.Tests.AuthenticationSteps
{
    public class AuthenticateWithFacebookAuthenticationStep : AuthenticationStep
    {
        private readonly string _facebookUserId;
        private readonly string _email;

        public AuthenticateWithFacebookAuthenticationStep(string facebookUserId, string email)
        {
            _facebookUserId = facebookUserId;
            _email = email;
        }
        
        public override UserDTO Run(AuthenticationService authenticationService)
        {
            return authenticationService.AuthenticateWithFacebook(_facebookUserId, _email);
        }
        
        public override string ToString()
        {
            return "Facebook";
        }
    }
}