using EntityFrameworkInheritanceExperiment.Service;

namespace EntityFrameworkInheritanceExperiment.Tests.AuthenticationSteps
{
    public class ThereShouldBeUserWithId : IContextRequirement
    {
        private readonly int _userId;

        public ThereShouldBeUserWithId(int userId)
        {
            _userId = userId;
        }

        public void Check(AuthenticationService authenticationService)
        {
            authenticationService.GetUser(_userId);
        }
    }
}