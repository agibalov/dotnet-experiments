using System.ServiceModel;
using NUnit.Framework;

namespace WcfExperiment
{
    public class MessageContractTest
    {
        [Test]
        public void CanUseMessageContract()
        {
            var calculatorService = new CalculatorService();
            using (var serviceHost = new ServiceHost(calculatorService))
            {
                serviceHost.AddServiceEndpoint(typeof (ICalculatorService), new BasicHttpBinding(), "http://localhost:2302/");
                serviceHost.Open();

                using (var channelFactory = new ChannelFactory<ICalculatorService>(new BasicHttpBinding(), "http://localhost:2302/"))
                {
                    var calculatoServiceClient = channelFactory.CreateChannel();
                    var response = calculatoServiceClient.AddNumbers(new AddNumbersRequest
                    {
                        A = 2,
                        B = 3
                    });
                    Assert.AreEqual(5, response.Result);
                }
            }
        }

        [ServiceContract]
        public interface ICalculatorService
        {
            [OperationContract]
            AddNumbersResponse AddNumbers(AddNumbersRequest request);
        }

        [MessageContract]
        public class AddNumbersRequest
        {
            [MessageHeader]
            public int A { get; set; }
            
            [MessageBodyMember]
            public int B { get; set; }
        }

        [MessageContract]
        public class AddNumbersResponse
        {
            [MessageBodyMember]
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
