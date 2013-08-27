using EntityFrameworkInheritanceExperiment.Service;

namespace EntityFrameworkInheritanceExperiment.Tests.AuthenticationSteps
{
    public interface IContextRequirement
    {
        void Check(AuthenticationService authenticationService);
    }
}