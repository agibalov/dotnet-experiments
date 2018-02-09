using System;
using Ninject;
using RestSharp;

namespace WpfWebApiExperiment.WebApiClient
{
    public class ApiClient : IApiClient
    {
        private readonly IRestClient _restClient;

        [Inject]
        public ApiClient(IRestClient restClient)
        {
            _restClient = restClient;
        }

        public TResult Execute<TResult>(IApiRequest<TResult> apiRequest)
            where TResult : new()
        {
            var restRequest = apiRequest.MakeRequest();
            var response = _restClient.Execute<TResult>(restRequest);
            
            var responseStatus = response.ResponseStatus;
            if (responseStatus == ResponseStatus.Completed)
            {
                try
                {
                    var result = apiRequest.HandleResponse(response);
                    return result;
                }
                catch (CantHandleResponse)
                {
                    throw new ApiException("Don't know how to handle " + response.StatusCode);
                }
            }

            if (responseStatus == ResponseStatus.Error)
            {
                throw new ConnectivityApiException(response.ErrorMessage);
            }

            throw new Exception("Unknown ApiClient error, please review the code");
        }
    }
}
