using System.Collections;
using System.Collections.Generic;
using Combinatorics.Collections;
using EntityFrameworkInheritanceExperiment.Tests.AuthenticationSteps;
using NUnit.Framework;

namespace EntityFrameworkInheritanceExperiment.Tests
{
    public class AuthenticationScenarioTests : AbstractAuthenticationServiceTests
    {
        [TestCaseSource("TestCases")]
        public void AuthenticationWorks(IList<AuthenticationStep> stepSequenceToConsider)
        {
            var contextRequirements = new List<IContextRequirement>();

            int? userId = null;
            foreach (var authenticationStep in stepSequenceToConsider)
            {
                var result = authenticationStep.Run(Service, contextRequirements);
                Assert.That(result, Is.Not.Null); // TODO

                if (!userId.HasValue)
                {
                    userId = result.UserId;
                }
                else
                {
                    Assert.That(result.UserId, Is.EqualTo(userId.Value));
                }
            }

            foreach (var contextRequirement in contextRequirements)
            {
                contextRequirement.Check(Service);
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
                            yield return new TestCaseData(stepSequenceToConsider).SetName(string.Join("->", stepSequenceToConsider));
                        }
                    }
                }
            }
        }
    }
}