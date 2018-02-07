using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
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

        [Test]
        public void CanGetBasicHttpBindingConnectivityError()
        {
            using (var channelFactory = new ChannelFactory<ICalculatorService>(new BasicHttpBinding(), "http://localhost:2302/"))
            {
                var calculatorServiceClient = channelFactory.CreateChannel();
                try
                {
                    calculatorServiceClient.AddNumbers(2, 3);
                    Assert.Fail();
                }
                catch (EndpointNotFoundException e)
                {
                    Assert.IsInstanceOf<WebException>(e.InnerException);
                    Assert.IsInstanceOf<SocketException>(e.InnerException.InnerException);
                }

                Assert.AreEqual(CommunicationState.Opened, channelFactory.State);
            }
        }

        [Test]
        public void CanGetNamedPipeConnectivityError()
        {
            // There's a different between NetNamedPipeBinding and BasicHttpBinding:
            // here, I can't use "using", because Dispose() throws CommunicationObjectFaultedException

            /*using (*/
            var channelFactory = new ChannelFactory<ICalculatorService>(new NetNamedPipeBinding(),
                "net.pipe://localhost/" /*)*/);
            {
                var calculatorServiceClient = channelFactory.CreateChannel();
                try
                {
                    calculatorServiceClient.AddNumbers(2, 3);
                    Assert.Fail();
                }
                catch (EndpointNotFoundException e)
                {
                    Assert.IsInstanceOf<PipeException>(e.InnerException);
                    Assert.IsNull(e.InnerException.InnerException);
                }

                Assert.AreEqual(CommunicationState.Opened, channelFactory.State);
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
