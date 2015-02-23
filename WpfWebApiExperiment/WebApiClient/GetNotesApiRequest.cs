using System.Collections.Generic;
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
    }
}