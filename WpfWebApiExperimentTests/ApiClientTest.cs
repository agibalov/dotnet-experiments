using NUnit.Framework;
using RestSharp;
using WpfWebApiExperiment.WebApiClient;

namespace WpfWebApiExperimentTests
{
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
