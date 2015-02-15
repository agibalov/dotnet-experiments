using System;
using System.Runtime.Serialization;
using System.ServiceModel;
using NUnit.Framework;

namespace WcfExperiment
{
    public class DataContractPolymorphicTest
    {
        [Test]
        public void CanAddNumbers()
        {
            WithClient(client =>
            {
                var response = client.Process(new AddNumbersRequest
                {
                    A = 2, 
                    B = 3
                });

                Assert.IsInstanceOf<OkResponse>(response);
                var okResponse = (OkResponse)response;
                Assert.AreEqual(5, okResponse.Result);
            });
        }

        [Test]
        public void CanDivideNumbers()
        {
            WithClient(client =>
            {
                var response = client.Process(new DivNumbersRequest
                {
                    A = 8, 
                    B = 2
                });

                Assert.IsInstanceOf<OkResponse>(response);
                var okResponse = (OkResponse)response;
                Assert.AreEqual(4, okResponse.Result);
            });
        }

        [Test]
        public void CanGetDivisionByZero()
        {
            WithClient(client =>
            {
                var response = client.Process(new DivNumbersRequest
                {
                    A = 8, 
                    B = 0
                });

                Assert.IsInstanceOf<DivisionByZeroResponse>(response);
            });
        }

        private static void WithClient(Action<ICalculatorService> clientAction)
        {
            var calculatorService = new CalculatorService();
            using (var serviceHost = new ServiceHost(calculatorService))
            {
                serviceHost.AddServiceEndpoint(typeof(ICalculatorService), new BasicHttpBinding(), "http://localhost:2302/");
                serviceHost.Open();

                using (var channelFactory = new ChannelFactory<ICalculatorService>(new BasicHttpBinding(), "http://localhost:2302/"))
                {
                    var calculatoServiceClient = channelFactory.CreateChannel();
                    clientAction(calculatoServiceClient);
                }
            }
        }

        [ServiceContract]
        public interface ICalculatorService
        {
            [OperationContract]
            CalculatorResponse Process(CalculatorRequest calculatorRequest);
        }

        [DataContract]
        [KnownType(typeof(AddNumbersRequest))]
        [KnownType(typeof(DivNumbersRequest))]
        public abstract class CalculatorRequest
        {
            [DataMember]
            public int A { get; set; }

            [DataMember]
            public int B { get; set; }
        }

        [DataContract]
        public class AddNumbersRequest : CalculatorRequest
        {
        }

        [DataContract]
        public class DivNumbersRequest : CalculatorRequest
        {
        }

        [DataContract]
        [KnownType(typeof(OkResponse))]
        [KnownType(typeof(DivisionByZeroResponse))]
        public abstract class CalculatorResponse
        {
        }

        [DataContract]
        public class OkResponse : CalculatorResponse
        {
            [DataMember]
            public int Result { get; set; }
        }

        [DataContract]
        public class DivisionByZeroResponse : CalculatorResponse
        {
        }

        [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
        public class CalculatorService : ICalculatorService
        {
            public CalculatorResponse Process(CalculatorRequest calculatorRequest)
            {
                if (calculatorRequest is AddNumbersRequest)
                {
                    var addNumbersRequest = (AddNumbersRequest) calculatorRequest;
                    return new OkResponse
                    {
                        Result = addNumbersRequest.A + addNumbersRequest.B
                    };
                }
                
                if (calculatorRequest is DivNumbersRequest)
                {
                    var divNumbersRequest = (DivNumbersRequest) calculatorRequest;
                    if (divNumbersRequest.B == 0)
                    {
                        return new DivisionByZeroResponse();
                    }

                    return new OkResponse
                    {
                        Result = divNumbersRequest.A/divNumbersRequest.B
                    };
                }
                
                throw new Exception();
            }
        }
    }
}
