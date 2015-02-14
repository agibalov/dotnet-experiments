using System.ServiceModel;
using NUnit.Framework;

namespace WcfExperiment
{
    public class DummyCalculatorTest
    {
        [Test]
        public void CanUseBasicHttpBinding()
        {
            var calculatorService = new CalculatorService();
            var serviceHost = new ServiceHost(calculatorService);
            serviceHost.AddServiceEndpoint(typeof(ICalculatorService), new BasicHttpBinding(), "http://localhost:2302/");
            serviceHost.Open();
            try
            {
                var channelFactory = new ChannelFactory<ICalculatorService>(new BasicHttpBinding(), "http://localhost:2302/");
                var calculatoServiceClient = channelFactory.CreateChannel();
                Assert.AreEqual(5, calculatoServiceClient.AddNumbers(2, 3));
            }
            finally
            {
                serviceHost.Close();
            }
        }

        [Test]
        public void CanUseNamedPipeBinding()
        {
            var calculatorService = new CalculatorService();
            var serviceHost = new ServiceHost(calculatorService);
            serviceHost.AddServiceEndpoint(typeof(ICalculatorService), new NetNamedPipeBinding(), "net.pipe://localhost/");
            serviceHost.Open();
            try
            {
                var channelFactory = new ChannelFactory<ICalculatorService>(new NetNamedPipeBinding(), "net.pipe://localhost/");
                var calculatoServiceClient = channelFactory.CreateChannel();
                Assert.AreEqual(5, calculatoServiceClient.AddNumbers(2, 3));
            }
            finally
            {
                serviceHost.Close();
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
