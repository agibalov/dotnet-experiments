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
            var response = new Mock<IRestResponse<List<NoteDTO>>>(MockBehavior.Strict);
            response.SetupProperty(r => r.ResponseStatus, ResponseStatus.Error);
            response.SetupProperty(r => r.ErrorMessage, "Something bad has happened");

            var restClient = new Mock<IRestClient>(MockBehavior.Strict);
            restClient.Setup(c => c.Execute<List<NoteDTO>>(It.Is<IRestRequest>(r =>
                r.Method == Method.GET && r.Resource == "/Notes"))).Returns(response.Object);
            var apiClient = new ApiClient(restClient.Object);

            try
            {
                apiClient.GetNotes();
                Assert.Fail();
            }
            catch (ApiClientException)
            {
            }

            restClient.Verify(c => c.Execute<List<NoteDTO>>(It.IsAny<IRestRequest>()), Times.Once);
            response.VerifyGet(r => r.ResponseStatus, Times.AtLeastOnce);
            response.VerifyGet(r => r.ErrorMessage, Times.Once);
        }

        [Test]
        public void ApiClientThrowsWhenRequestFailsBecauseOfNonOkResponse()
        {
            var response = new Mock<IRestResponse<List<NoteDTO>>>(MockBehavior.Strict);
            response.SetupProperty(r => r.ResponseStatus, ResponseStatus.Completed);
            response.SetupProperty(r => r.StatusCode, HttpStatusCode.InternalServerError);

            var restClient = new Mock<IRestClient>(MockBehavior.Strict);
            restClient.Setup(c => c.Execute<List<NoteDTO>>(It.Is<IRestRequest>(r =>
                r.Method == Method.GET && r.Resource == "/Notes"))).Returns(response.Object);
            var apiClient = new ApiClient(restClient.Object);

            try
            {
                apiClient.GetNotes();
                Assert.Fail();
            }
            catch (ApiException)
            {
            }

            restClient.Verify(c => c.Execute<List<NoteDTO>>(It.IsAny<IRestRequest>()), Times.Once);
            response.VerifyGet(r => r.ResponseStatus, Times.Once);
            response.VerifyGet(r => r.StatusCode, Times.Once);
        }

        [Test]
        public void ApiClientDoesNotThrowWhenResponseIsOk()
        {
            var response = new Mock<IRestResponse<List<NoteDTO>>>(MockBehavior.Strict);
            response.SetupProperty(r => r.ResponseStatus, ResponseStatus.Completed);
            response.SetupProperty(r => r.StatusCode, HttpStatusCode.OK);
            response.SetupProperty(r => r.Data, new List<NoteDTO>
            {
                new NoteDTO {Id = "123", Title = "Hi", Text = "Hello there"}
            });

            var restClient = new Mock<IRestClient>(MockBehavior.Strict);
            restClient.Setup(c => c.Execute<List<NoteDTO>>(It.Is<IRestRequest>(r => 
                r.Method == Method.GET && r.Resource == "/Notes"))).Returns(response.Object);
            var apiClient = new ApiClient(restClient.Object);

            var notes = apiClient.GetNotes();
            Assert.AreEqual(1, notes.Count);

            restClient.Verify(c => c.Execute<List<NoteDTO>>(It.IsAny<IRestRequest>()), Times.Once);
            response.VerifyGet(r => r.ResponseStatus, Times.Once);
            response.VerifyGet(r => r.StatusCode, Times.Once);
            response.VerifyGet(r => r.Data, Times.Once);
        }
    }
}
