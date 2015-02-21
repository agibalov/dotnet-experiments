using NUnit.Framework;
using RestSharp;
using WpfWebApiExperiment.WebApiClient;

namespace WpfWebApiExperimentTests
{
    // +TODO: how do I test a transport error?
    // TODO: how do I test 200/400/500 responses?

    public class ApiClientTest
    {
        [Test]
        public void ApiClientThrowsWhenRequestFailsBecauseOfTransportError()
        {
            var restClient = new RestClient("http://localhost:8080/");
            var apiClient = new ApiClient(restClient);

            try
            {
                apiClient.GetNotes();
                Assert.Fail();
            }
            catch (ApiClientException)
            {
            }
        }
    }
}
