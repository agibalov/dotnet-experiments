using System;
using System.Net;
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

        public TResult Execute<TResult>(IRestRequest restRequest)
            where TResult : new()
        {
            var response = _restClient.Execute<TResult>(restRequest);
            
            var responseStatus = response.ResponseStatus;
            if (responseStatus == ResponseStatus.Completed)
            {
                var httpStatusCode = response.StatusCode;
                if (httpStatusCode == HttpStatusCode.OK)
                {
                    return response.Data;
                }

                throw new ApiException("Unexpected StatusCode, please review the code");
            }

            // for example, connectivity error
            if (responseStatus == ResponseStatus.Error)
            {
                throw new ApiClientException(response.ErrorMessage);
            }

            throw new Exception("Unknown ApiClient error, please review the code");
        }
    }

    public class ApiClientException : Exception
    {
        public ApiClientException(string message)
            : base(message)
        {
        }
    }

    public class ApiException : ApiClientException
    {
        public ApiException(string message) 
            : base(message)
        {
        }
    }
}
