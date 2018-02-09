using NUnit.Framework;
using SophisticatedDataValidationExperiment.Service;

namespace SophisticatedDataValidationExperiment
{
    public class MegaServiceTests : AbstractTests
    {
        [Test]
        public void Test()
        {
            var service = new MegaService();
            var order = service.GetOrder();
            AssertAdequateOrder(order);
        }
    }
}