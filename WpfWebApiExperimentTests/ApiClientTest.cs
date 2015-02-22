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
            var restClient = MakeResponseStatusErrorRestClientMock();
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
        }

        [Test]
        public void ApiClientThrowsWhenRequestFailsBecauseOfNonOkResponse()
        {
            var restClient = MakeInternalServerErrorRestClientMock();
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
        }

        [Test]
        public void ApiClientDoesNotThrowWhenResponseIsOk()
        {
            var restClient = MakeOkRestClientMock(new List<NoteDTO>
            {
                new NoteDTO {Id = "123", Title = "Hi", Text = "Hello there"}
            });
            var apiClient = new ApiClient(restClient.Object);

            var notes = apiClient.GetNotes();
            Assert.AreEqual(1, notes.Count);

            restClient.Verify(c => c.Execute<List<NoteDTO>>(It.IsAny<IRestRequest>()), Times.Once);
        }

        // TODO: should I mock the responses to make sure that ApiClient only accesses the fields I expect it to access?
        private static Mock<IRestClient> MakeResponseStatusErrorRestClientMock()
        {
            var restClient = new Mock<IRestClient>(MockBehavior.Strict);
            restClient.Setup(c => c.Execute<List<NoteDTO>>(It.IsAny<IRestRequest>()))
                .Returns(new RestResponse<List<NoteDTO>>
                {
                    ResponseStatus = ResponseStatus.Error
                });

            return restClient;
        }

        // TODO: should I mock the responses to make sure that ApiClient only accesses the fields I expect it to access?
        private static Mock<IRestClient> MakeInternalServerErrorRestClientMock()
        {
            var restClient = new Mock<IRestClient>(MockBehavior.Strict);
            restClient.Setup(c => c.Execute<List<NoteDTO>>(It.IsAny<IRestRequest>()))
                .Returns(new RestResponse<List<NoteDTO>>
                {
                    ResponseStatus = ResponseStatus.Completed,
                    StatusCode = HttpStatusCode.InternalServerError
                });

            return restClient;
        }

        // TODO: should I mock the responses to make sure that ApiClient only accesses the fields I expect it to access?
        private static Mock<IRestClient> MakeOkRestClientMock(List<NoteDTO> notes)
        {
            var restClient = new Mock<IRestClient>(MockBehavior.Strict);
            restClient.Setup(c => c.Execute<List<NoteDTO>>(It.IsAny<IRestRequest>()))
                .Returns(new RestResponse<List<NoteDTO>>
                {
                    ResponseStatus = ResponseStatus.Completed,
                    StatusCode = HttpStatusCode.OK,
                    Data = notes
                });

            return restClient;
        }
    }
}
