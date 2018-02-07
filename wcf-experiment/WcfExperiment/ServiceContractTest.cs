using System.ServiceModel;
using NUnit.Framework;

namespace WcfExperiment
{
    public class ServiceContractTest
    {
        [Test]
        public void CanUseServiceContract()
        {
            var calculatorService = new CalculatorService();
            using (var serviceHost = new ServiceHost(calculatorService))
            {
                serviceHost.AddServiceEndpoint(typeof (ICalculatorService), new BasicHttpBinding(), "http://localhost:2302/");
                serviceHost.Open();
                using (var channelFactory = new ChannelFactory<ICalculatorService>(new BasicHttpBinding(), "http://localhost:2302/"))
                {
                    var calculatorServiceClient = channelFactory.CreateChannel();
                    Assert.AreEqual(5, calculatorServiceClient.AddNumbers(2, 3));
                }
            }
        }

        [ServiceContract]
        public interface ICalculatorService
        {
            [OperationContract]
            int AddNumbers(int a, int b);
        }

        [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
        public class CalculatorService : ICalculatorService
        {
            public int AddNumbers(int a, int b)
            {
                return a + b;
            }
        }
    }
}
