using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace FluentValidationExperiment
{
    [TestClass]
    public class ValidationTests
    {
        private readonly UserValidator _userValidator = new UserValidator();

        [TestMethod]
        public void GoodUserIsValid()
        {
            var user = new User
                {
                    UserName = "loki2302", 
                    Email = "loki2302@loki2302.com", 
                    Password = "qwerty123"
                };
            
            var validationResult = _userValidator.Validate(user);
            Assert.IsTrue(validationResult.IsValid);
        }

        [TestMethod]
        public void ThrowsOnEmptyUserName()
        {
            var user = new User
                {
                    UserName = null,
                    Email = "loki2302@loki2302.com",
                    Password = "qwerty123"
                };

            var validationResult = _userValidator.Validate(user);
            Assert.IsFalse(validationResult.IsValid);
            Assert.AreEqual(1, validationResult.Errors.Count);
            Assert.AreEqual(null, validationResult.Errors[0].AttemptedValue);
            Assert.AreEqual("UserName", validationResult.Errors[0].PropertyName);
        }

        [TestMethod]
        public void ThrowsOnEmptyEmail()
        {
            var user = new User
            {
                UserName = "loki2302",
                Email = null,
                Password = "qwerty123"
            };

            var validationResult = _userValidator.Validate(user);
            Assert.IsFalse(validationResult.IsValid);
            Assert.AreEqual(1, validationResult.Errors.Count);
            Assert.AreEqual(null, validationResult.Errors[0].AttemptedValue);
            Assert.AreEqual("Email", validationResult.Errors[0].PropertyName);
        }

        [TestMethod]
        public void ThrowsOnEmptyPassword()
        {
            var user = new User
            {
                UserName = "loki2302",
                Email = "loki2302@loki2302.com",
                Password = null
            };

            var validationResult = _userValidator.Validate(user);
            Assert.IsFalse(validationResult.IsValid);
            Assert.AreEqual(1, validationResult.Errors.Count);
            Assert.AreEqual(null, validationResult.Errors[0].AttemptedValue);
            Assert.AreEqual("Password", validationResult.Errors[0].PropertyName);
        }

        [TestMethod]
        public void ThrowsOnEmptyEverything()
        {
            var user = new User
            {
                UserName = null,
                Email = null,
                Password = null
            };

            var validationResult = _userValidator.Validate(user);
            Assert.IsFalse(validationResult.IsValid);
            Assert.AreEqual(3, validationResult.Errors.Count);
            Assert.AreEqual(1, validationResult.Errors.Count(e => e.PropertyName == "UserName"));
            Assert.AreEqual(1, validationResult.Errors.Count(e => e.PropertyName == "Email"));
            Assert.AreEqual(1, validationResult.Errors.Count(e => e.PropertyName == "Password"));
        }

        [TestMethod]
        public void ThrowsOnShortPassword()
        {
            var user = new User
            {
                UserName = "loki2302",
                Email = "loki2302@loki2302.com",
                Password = "abc"
            };

            var validationResult = _userValidator.Validate(user);
            Assert.IsFalse(validationResult.IsValid);
            Assert.AreEqual(1, validationResult.Errors.Count);
            Assert.AreEqual(1, validationResult.Errors.Count(e => e.PropertyName == "Password"));
        }

        [TestMethod]
        public void ThrowsOnLongPassword()
        {
            var user = new User
            {
                UserName = "loki2302",
                Email = "loki2302@loki2302.com",
                Password = "123456789012345678901234567890123"
            };

            var validationResult = _userValidator.Validate(user);
            Assert.IsFalse(validationResult.IsValid);
            Assert.AreEqual(1, validationResult.Errors.Count);
            Assert.AreEqual(1, validationResult.Errors.Count(e => e.PropertyName == "Password"));
        }
    }
}