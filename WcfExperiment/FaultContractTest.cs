using System;
using System.Runtime.Serialization;
using System.ServiceModel;
using NUnit.Framework;

namespace WcfExperiment
{
    public class FaultContractTest
    {
        [Test]
        public void CanGetAnExpectedFailure()
        {
            var calculatorService = new FailingService();
            var serviceHost = new ServiceHost(calculatorService);
            serviceHost.AddServiceEndpoint(typeof(IFailingService), new BasicHttpBinding(), "http://localhost:2302/");
            serviceHost.Open();
            try
            {
                var channelFactory = new ChannelFactory<IFailingService>(new BasicHttpBinding(), "http://localhost:2302/");
                var calculatorServiceClient = channelFactory.CreateChannel();

                try
                {
                    calculatorServiceClient.PleaseFailExpectedly();
                    Assert.Fail();
                }
                catch (FaultException<MyFault> e)
                {
                    Assert.AreEqual("omgwtfbbq", e.Detail.Message);
                    Assert.AreEqual("Client", e.Code.Name);
                    Assert.IsTrue(e.Code.IsPredefinedFault);
                    Assert.IsFalse(e.Code.IsReceiverFault);
                    Assert.IsTrue(e.Code.IsSenderFault);
                }
            }
            finally
            {
                serviceHost.Close();
            }
        }

        [Test]
        public void CanGetAnUnexpectedFailure()
        {
            var calculatorService = new FailingService();
            var serviceHost = new ServiceHost(calculatorService);
            serviceHost.AddServiceEndpoint(typeof(IFailingService), new BasicHttpBinding(), "http://localhost:2302/");
            serviceHost.Open();
            try
            {
                var channelFactory = new ChannelFactory<IFailingService>(new BasicHttpBinding(), "http://localhost:2302/");
                var calculatorServiceClient = channelFactory.CreateChannel();

                try
                {
                    calculatorServiceClient.PleaseFailUnexpectedly();
                    Assert.Fail();
                }
                catch (FaultException e)
                {
                    Assert.AreEqual("InternalServiceFault", e.Code.Name);
                    Assert.IsFalse(e.Code.IsPredefinedFault);
                    Assert.IsFalse(e.Code.IsReceiverFault);
                    Assert.IsFalse(e.Code.IsSenderFault);
                }
            }
            finally
            {
                serviceHost.Close();
            }
        }

        [ServiceContract]
        public interface IFailingService
        {
            [OperationContract]
            [FaultContract(typeof(MyFault))]
            int PleaseFailExpectedly();

            [OperationContract]
            int PleaseFailUnexpectedly();
        }

        [DataContract]
        public class MyFault
        {
            [DataMember]
            public string Message { get; set; }
        }

        [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
        public class FailingService : IFailingService
        {
            public int PleaseFailExpectedly()
            {
                throw new FaultException<MyFault>(new MyFault
                {
                    Message = "omgwtfbbq"
                });
            }

            public int PleaseFailUnexpectedly()
            {
                throw new Exception("wat?");
            }
        }
    }
}
