using EntityFrameworkInheritanceExperiment.DTO;
using EntityFrameworkInheritanceExperiment.Service;

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

        public override UserDTO Run(AuthenticationService authenticationService)
        {
            return authenticationService.SignUpWithEmailAndPassword(_email, _password);
        }

        public override string ToString()
        {
            return "Password";
        }
    }
}