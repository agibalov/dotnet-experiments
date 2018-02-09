using System.Collections.Generic;
using System.Net;
using Moq;
using NUnit.Framework;
using RestSharp;
using WpfWebApiExperiment.WebApi;
using WpfWebApiExperiment.WebApiClient;

namespace WpfWebApiExperimentTests
{
    public class ApiClientTest
    {
        [Test]
        public void ApiClientThrowsWhenRequestFailsBecauseOfTransportError()
        {
            //var apiRequest = new Mock<IApiRequest<List<NoteDTO>>>(MockBehavior.Strict);
            
            var restResponse = new Mock<IRestResponse<List<NoteDTO>>>(MockBehavior.Strict);
            restResponse.SetupProperty(r => r.ResponseStatus, ResponseStatus.Error);
            restResponse.SetupProperty(r => r.ErrorMessage, "Something bad has happened");

            var restClient = new Mock<IRestClient>(MockBehavior.Strict);
            restClient.Setup(c => c.Execute<List<NoteDTO>>(It.IsAny<IRestRequest>())).Returns(restResponse.Object);

            var apiClient = new ApiClient(restClient.Object);

            try
            {
                apiClient.Execute(new GetNotesApiRequest());
                Assert.Fail();
            }
            catch (ConnectivityApiException)
            {
            }

            restClient.Verify(c => c.Execute<List<NoteDTO>>(It.IsAny<IRestRequest>()), Times.Once);
            restResponse.VerifyGet(r => r.ResponseStatus, Times.Once);
            restResponse.VerifyGet(r => r.ErrorMessage, Times.Once);
        }

        [Test]
        public void ApiClientThrowsWhenRequestFailsBecauseOfNonOkResponse()
        {
            var restResponse = new Mock<IRestResponse<List<NoteDTO>>>(MockBehavior.Strict);
            restResponse.SetupProperty(r => r.ResponseStatus, ResponseStatus.Completed);
            restResponse.SetupProperty(r => r.StatusCode, HttpStatusCode.InternalServerError);

            var restClient = new Mock<IRestClient>(MockBehavior.Strict);
            restClient.Setup(c => c.Execute<List<NoteDTO>>(It.IsAny<IRestRequest>())).Returns(restResponse.Object);

            var apiClient = new ApiClient(restClient.Object);

            try
            {
                apiClient.Execute(new GetNotesApiRequest());
                Assert.Fail();
            }
            catch (ApiException)
            {
            }

            restClient.Verify(c => c.Execute<List<NoteDTO>>(It.IsAny<IRestRequest>()), Times.Once);
            restResponse.VerifyGet(r => r.ResponseStatus, Times.AtLeastOnce);
            restResponse.VerifyGet(r => r.StatusCode, Times.AtLeastOnce);
        }

        [Test]
        public void ApiClientDoesNotThrowWhenResponseIsOk()
        {
            var restResponse = new Mock<IRestResponse<List<NoteDTO>>>(MockBehavior.Strict);
            restResponse.SetupProperty(r => r.ResponseStatus, ResponseStatus.Completed);
            restResponse.SetupProperty(r => r.StatusCode, HttpStatusCode.OK);
            restResponse.SetupProperty(r => r.Data, new List<NoteDTO>
            {
                new NoteDTO {Id = "123", Title = "Hi", Text = "Hello there"}
            });

            var restClient = new Mock<IRestClient>(MockBehavior.Strict);
            restClient.Setup(c => c.Execute<List<NoteDTO>>(It.IsAny<IRestRequest>())).Returns(restResponse.Object);

            var apiClient = new ApiClient(restClient.Object);

            var notes = apiClient.Execute(new GetNotesApiRequest());
            Assert.AreEqual(1, notes.Count);

            restClient.Verify(c => c.Execute<List<NoteDTO>>(It.IsAny<IRestRequest>()), Times.Once);
            restResponse.VerifyGet(r => r.ResponseStatus, Times.AtLeastOnce);
            restResponse.VerifyGet(r => r.StatusCode, Times.Once);
            restResponse.VerifyGet(r => r.Data, Times.Once);
        }
    }
}
