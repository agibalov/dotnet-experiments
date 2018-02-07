using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Combinatorics.Collections;
using EntityFrameworkInheritanceExperiment.Tests.AuthenticationSteps;
using EntityFrameworkInheritanceExperiment.Tests.Expectations;
using NUnit.Framework;

namespace EntityFrameworkInheritanceExperiment.Tests
{
    public class AuthenticationScenarioTests : AbstractAuthenticationServiceTests
    {
        [TestCaseSource("TestCases")]
        public void AuthenticationWorks(IList<AuthenticationStep> stepSequenceToConsider, IList<IExpectation> expectations)
        {
            foreach (var authenticationStep in stepSequenceToConsider)
            {
                authenticationStep.Run(Service);
            }

            foreach (var expectation in expectations)
            {
                expectation.Check(Service);
            }
        }

        public static IEnumerable TestCases
        {
            get
            {
                var authenticationSteps = new List<AuthenticationStep>
                    {
                        new SignUpWithEmailAndPasswordAuthenticationStep("loki2302@loki2302.me", "qwerty"),
                        new AuthenticateWithGoogleAuthenticationStep("google1", "loki2302@loki2302.me"),
                        new AuthenticateWithFacebookAuthenticationStep("facebook1", "loki2302@loki2302.me"),
                        new AuthenticateWithTwitterAuthenticationStep("twitter1", "loki2302")
                    };

                for (var numberOfItemsToConsider = 1; numberOfItemsToConsider <= authenticationSteps.Count; ++numberOfItemsToConsider)
                {
                    var stepSetsToConsider = new Combinations<AuthenticationStep>(authenticationSteps, numberOfItemsToConsider);
                    foreach (var stepSetToConsider in stepSetsToConsider)
                    {
                        var stepSequencesToConsider = new Permutations<AuthenticationStep>(stepSetToConsider);
                        foreach (var stepSequenceToConsider in stepSequencesToConsider)
                        {
                            var expectations = BuildExpectations(stepSequenceToConsider);
                            yield return new TestCaseData(stepSequenceToConsider, expectations).SetName(string.Join("->", stepSequenceToConsider));
                        }
                    }
                }
            }
        }

        private static IList<IExpectation> BuildExpectations(IList<AuthenticationStep> authenticationSteps)
        {
            var expectations = new List<IExpectation>();

            var shouldHaveOneNonTwitterUser = authenticationSteps.Any(authenticationStep =>
                authenticationStep is SignUpWithEmailAndPasswordAuthenticationStep ||
                authenticationStep is AuthenticateWithGoogleAuthenticationStep ||
                authenticationStep is AuthenticateWithFacebookAuthenticationStep);
            if (shouldHaveOneNonTwitterUser)
            {
                expectations.Add(new ThereIsOneNonTwitterUserExpectation());
            }

            var nonTwitterUserShouldHaveEmailAndPasswordAuthMethod = authenticationSteps.Any(authenticationStep =>
                    authenticationStep is SignUpWithEmailAndPasswordAuthenticationStep);
            if (nonTwitterUserShouldHaveEmailAndPasswordAuthMethod)
            {
                expectations.Add(new NonTwitterUserHasEmailAndPasswordAuthMethod());
            }

            var nonTwitterUserShouldHaveGoogleAuthMethod = authenticationSteps.Any(authenticationStep =>
                    authenticationStep is AuthenticateWithGoogleAuthenticationStep);
            if (nonTwitterUserShouldHaveGoogleAuthMethod)
            {
                expectations.Add(new NonTwitterUserHasGoogleAuthMethod());
            }

            var nonTwitterUserShouldHaveFacebookAuthMethod = authenticationSteps.Any(authenticationStep =>
                    authenticationStep is AuthenticateWithFacebookAuthenticationStep);
            if (nonTwitterUserShouldHaveFacebookAuthMethod)
            {
                expectations.Add(new NonTwitterUserHasFacebookAuthMethod());
            }

            var shouldHaveOneTwitterUser = authenticationSteps.Any(authenticationStep =>
                authenticationStep is AuthenticateWithTwitterAuthenticationStep);
            if (shouldHaveOneTwitterUser)
            {
                expectations.Add(new ThereIsOneTwitterUserExpectation());
            }

            return expectations;
        }
    }
}