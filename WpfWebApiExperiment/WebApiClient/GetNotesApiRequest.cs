using System;
using System.Collections.Generic;
using System.Net;
using RestSharp;
using WpfWebApiExperiment.WebApi;

namespace WpfWebApiExperiment.WebApiClient
{
    public class GetNotesApiRequest : IApiRequest<List<NoteDTO>>
    {
        public IRestRequest MakeRequest()
        {
            var request = new RestRequest("/Notes", Method.GET);
            return request;
        }

        public List<NoteDTO> HandleResponse(IRestResponse<List<NoteDTO>> restResponse)
        {
            if (restResponse.ResponseStatus != ResponseStatus.Completed)
            {
                throw new ArgumentException();
            }

            var httpStatusCode = restResponse.StatusCode;
            if (httpStatusCode == HttpStatusCode.OK)
            {
                return restResponse.Data;
            }

            throw new CantHandleResponse();
        }
    }
}
