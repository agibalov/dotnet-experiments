using System;
using System.Collections.Generic;
using System.Net;
using Ninject;
using RestSharp;
using WpfWebApiExperiment.WebApi;

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

        public List<NoteDTO> GetNotes()
        {
            var request = new RestRequest("/Notes", Method.GET);
            var result = Execute<List<NoteDTO>>(request);
            return result;
        }

        public NoteDTO GetNote(string id)
        {
            var request = new RestRequest("/Notes/{id}", Method.GET);
            request.AddParameter("id", id);
            var result = Execute<NoteDTO>(request);
            return result;
        }

        private TResult Execute<TResult>(RestRequest restRequest)
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
