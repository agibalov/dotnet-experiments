using EntityFrameworkInheritanceExperiment.Service;

namespace EntityFrameworkInheritanceExperiment.Tests.Expectations
{
    public interface IExpectation
    {
        void Check(AuthenticationService authenticationService);
    }
}