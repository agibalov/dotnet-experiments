using AOPValidationExperiment.Validation;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Ninject;

namespace AOPValidationExperiment
{
    [TestClass]
    public class ValidationTests
    {
        private TestApi _api;

        [TestInitialize]
        public void SetUpApi()
        {
            var kernel = new StandardKernel();
            kernel.Bind<TestApi>().ToSelf();
            _api = kernel.Get<TestApi>();
        }

        [TestMethod]
        [ExpectedException(typeof(ValidationException))]
        public void Test1()
        {
            _api.SignIn(null, "b");
        }
    }
}
