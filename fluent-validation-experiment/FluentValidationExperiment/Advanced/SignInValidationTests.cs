using System.Linq;
using FluentValidationExperiment.Advanced.RequestValidators;
using FluentValidationExperiment.Advanced.Requests;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FluentValidationExperiment.Advanced
{
    [TestClass]
    public class SignInValidationTests
    {
        private readonly SignInRequestValidator _signInRequestValidator = new SignInRequestValidator();

        [TestMethod]
        public void IsValidWhenEmailAndPasswordAreOk()
        {
            var signInRequest = new SignInRequest
                {
                    Email = "loki2302@loki2302.loki2302",
                    Password = "qwerty123"
                };

            var validationResult = _signInRequestValidator.Validate(signInRequest);
            Assert.IsTrue(validationResult.IsValid);
        }

        [TestMethod]
        public void IsInvalidWhenEmailIsNull()
        {
            var signInRequest = new SignInRequest
                {
                    Email = null, 
                    Password = "qwerty123"
                };

            var validationResult = _signInRequestValidator.Validate(signInRequest);
            Assert.IsFalse(validationResult.IsValid);
            Assert.AreEqual(1, validationResult.Errors.Count);
            var error = validationResult.Errors[0];
            Assert.AreEqual("Email", error.PropertyName);
        }

        [TestMethod]
        public void IsInvalidWhenEmailIsEmpty()
        {
            var signInRequest = new SignInRequest
            {
                Email = string.Empty,
                Password = "qwerty123"
            };

            var validationResult = _signInRequestValidator.Validate(signInRequest);
            Assert.IsFalse(validationResult.IsValid);
            Assert.AreEqual(2, validationResult.Errors.Count);
            Assert.IsTrue(validationResult.Errors.All(error => error.PropertyName == "Email"));
        }

        [TestMethod]
        public void IsInvalidWhenEmailIsInvalid()
        {
            var signInRequest = new SignInRequest
            {
                Email = "loki2302",
                Password = "qwerty123"
            };

            var validationResult = _signInRequestValidator.Validate(signInRequest);
            Assert.IsFalse(validationResult.IsValid);
            Assert.AreEqual(1, validationResult.Errors.Count);
            var error = validationResult.Errors[0];
            Assert.AreEqual("Email", error.PropertyName);
        }

        [TestMethod]
        public void IsInvalidWhenPasswordIsNull()
        {
            var signInRequest = new SignInRequest
            {
                Email = "loki2302@loki2302.loki2302",
                Password = null
            };

            var validationResult = _signInRequestValidator.Validate(signInRequest);
            Assert.IsFalse(validationResult.IsValid);
            Assert.AreEqual(2, validationResult.Errors.Count);
            Assert.IsTrue(validationResult.Errors.All(error => error.PropertyName == "Password"));
        }

        [TestMethod]
        public void IsInvalidWhenPasswordIsEmpty()
        {
            var signInRequest = new SignInRequest
            {
                Email = "loki2302@loki2302.loki2302",
                Password = string.Empty
            };

            var validationResult = _signInRequestValidator.Validate(signInRequest);
            Assert.IsFalse(validationResult.IsValid);
            Assert.AreEqual(2, validationResult.Errors.Count);
            Assert.IsTrue(validationResult.Errors.All(error => error.PropertyName == "Password"));
        }

        [TestMethod]
        public void IsInvalidWhenPasswordIsShort()
        {
            var signInRequest = new SignInRequest
            {
                Email = "loki2302@loki2302.loki2302",
                Password = "12345"
            };

            var validationResult = _signInRequestValidator.Validate(signInRequest);
            Assert.IsFalse(validationResult.IsValid);
            Assert.AreEqual(1, validationResult.Errors.Count);
            Assert.IsTrue(validationResult.Errors.All(error => error.PropertyName == "Password"));
        }

        [TestMethod]
        public void IsInvalidWhenPasswordIsLong()
        {
            var signInRequest = new SignInRequest
            {
                Email = "loki2302@loki2302.loki2302",
                Password = "012345678901234567890123456789012"
            };

            var validationResult = _signInRequestValidator.Validate(signInRequest);
            Assert.IsFalse(validationResult.IsValid);
            Assert.AreEqual(1, validationResult.Errors.Count);
            Assert.IsTrue(validationResult.Errors.All(error => error.PropertyName == "Password"));
        }
    }
}
