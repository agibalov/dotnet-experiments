using System.ServiceModel;
using NUnit.Framework;

namespace WcfExperiment
{
    public class BindingsTest
    {
        [Test]
        public void CanUseBasicHttpBinding()
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

        [Test]
        public void CanUseNamedPipeBinding()
        {
            var calculatorService = new CalculatorService();
            using (var serviceHost = new ServiceHost(calculatorService))
            {
                serviceHost.AddServiceEndpoint(typeof (ICalculatorService), new NetNamedPipeBinding(), "net.pipe://localhost/");
                serviceHost.Open();

                using (var channelFactory = new ChannelFactory<ICalculatorService>(new NetNamedPipeBinding(), "net.pipe://localhost/"))
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
