using System.Runtime.Serialization;
using System.ServiceModel;
using NUnit.Framework;

namespace WcfExperiment
{
    public class DataContractTest
    {
        [Test]
        public void CanUseDataContract()
        {
            var calculatorService = new CalculatorService();
            var serviceHost = new ServiceHost(calculatorService);
            serviceHost.AddServiceEndpoint(typeof(ICalculatorService), new BasicHttpBinding(), "http://localhost:2302/");
            serviceHost.Open();
            try
            {
                var channelFactory = new ChannelFactory<ICalculatorService>(new BasicHttpBinding(), "http://localhost:2302/");
                var calculatoServiceClient = channelFactory.CreateChannel();
                var response = calculatoServiceClient.AddNumbers(new AddNumbersRequest
                {
                    A = 2,
                    B = 3
                });
                Assert.AreEqual(5, response.Result);
            }
            finally
            {
                serviceHost.Close();
            }
        }

        [ServiceContract]
        public interface ICalculatorService
        {
            [OperationContract]
            AddNumbersResponse AddNumbers(AddNumbersRequest request);
        }

        [DataContract]
        public class AddNumbersRequest
        {
            [DataMember]
            public int A { get; set; }
            
            [DataMember]
            public int B { get; set; }
        }

        [DataContract]
        public class AddNumbersResponse
        {
            [DataMember]
            public int Result { get; set; }
        }

        [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
        public class CalculatorService : ICalculatorService
        {
            public AddNumbersResponse AddNumbers(AddNumbersRequest request)
            {
                var a = request.A;
                var b = request.B;
                var sum = a + b;
                return new AddNumbersResponse
                {
                    Result = sum
                };
            }
        }
    }
}
