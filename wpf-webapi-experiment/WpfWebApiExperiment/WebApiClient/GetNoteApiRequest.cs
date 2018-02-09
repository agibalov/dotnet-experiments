using System;
using System.Net;
using RestSharp;
using WpfWebApiExperiment.WebApi;

namespace WpfWebApiExperiment.WebApiClient
{
    public class GetNoteApiRequest : IApiRequest<NoteDTO>
    {
        public string Id { get; set; }

        public IRestRequest MakeRequest()
        {
            var request = new RestRequest("/Notes/{id}", Method.GET);
            request.AddParameter("id", Id);
            return request;
        }

        public NoteDTO HandleResponse(IRestResponse<NoteDTO> restResponse)
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

            if (httpStatusCode == HttpStatusCode.NotFound)
            {
                throw new NoteNotFoundException(Id);
            }

            throw new CantHandleResponse();
        }
    }
}
