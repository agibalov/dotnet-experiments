using System.Linq;
using AOPValidationExperiment.Validation;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Ninject;
using Ninject.Extensions.Interception;
using Ninject.Extensions.Interception.Infrastructure.Language;

namespace AOPValidationExperiment
{
    [TestClass]
    public class ValidationTests
    {
        private TestApi _api;

        [TestInitialize]
        public void SetUpApi()
        {
            // wtf? it has been working without this extra stuff before installing VS2012 Update 2
            var kernel = new StandardKernel(new NinjectSettings { LoadExtensions = false }, new LinFuModule());
            kernel.Bind<TestApi>().ToSelf();
            _api = kernel.Get<TestApi>();
        }

        [TestMethod]
        public void SignInFailsWhenLoginIsInvalid()
        {
            try
            {
                _api.SignIn(null, "qwerty");
                Assert.Fail();
            }
            catch (ValidationException e)
            {
                Assert.AreEqual(1, e.FieldsInError.Count);
                Assert.AreEqual(e.FieldsInError[0], "userName");
            }
        }

        [TestMethod]
        public void SignInFailsWhenPasswordIsInvalid()
        {
            try
            {
                _api.SignIn("loki2302", null);
                Assert.Fail();
            }
            catch (ValidationException e)
            {
                Assert.AreEqual(1, e.FieldsInError.Count);
                Assert.AreEqual(e.FieldsInError[0], "password");
            }
        }

        [TestMethod]
        public void SignInFailsWhenBothUserNameAndPasswordAreInvalid()
        {
            try
            {
                _api.SignIn(null, null);
                Assert.Fail();
            }
            catch (ValidationException e)
            {
                Assert.AreEqual(2, e.FieldsInError.Count);
                Assert.IsTrue(e.FieldsInError.Any(fieldInError => fieldInError == "userName"));
                Assert.IsTrue(e.FieldsInError.Any(fieldInError => fieldInError == "password"));
            }
        }
    }
}
