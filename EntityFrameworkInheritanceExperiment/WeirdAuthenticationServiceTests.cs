using System;
using System.Collections;
using NUnit.Framework;

namespace EntityFrameworkInheritanceExperiment
{
    public class WeirdAuthenticationServiceTests : AbstractAuthenticationServiceTests
    {
        [Test]
        [TestCaseSource("TestCases")]
        public void Test()
        {
            throw new NotImplementedException();
        }

        public static IEnumerable TestCases
        {
            get
            {
                /*
                
                generate scenarios like:
                
                email-google
                email-facebook
                email-twitter
                email-google-facebook
                email-google-twitter
                email-facebook-google
                email-facebook-twitter
                email-twitter-google
                email-twitter-facebook
                
                */

                yield return new TestCaseData();
            }
        }
    }
}