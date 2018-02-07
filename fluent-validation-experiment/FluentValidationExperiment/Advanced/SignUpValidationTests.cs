using FluentValidationExperiment.Advanced.RequestValidators;
using FluentValidationExperiment.Advanced.Requests;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FluentValidationExperiment.Advanced
{
    [TestClass]
    public class SignUpValidationTests
    {
        private readonly SignUpRequestValidator _signUpRequestValidator = new SignUpRequestValidator();

        [TestMethod]
        public void IsValidWhenEverythingIsOk()
        {
            var signUpRequest = new SignUpRequest
                {
                    Email = "loki2302@loki2302.loki2302",
                    Password1 = "qwerty123",
                    Password2 = "qwerty123"
                };

            var validationResult = _signUpRequestValidator.Validate(signUpRequest);
            Assert.IsTrue(validationResult.IsValid);
        }
    }
}