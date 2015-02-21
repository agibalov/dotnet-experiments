using System.Collections.Generic;
using Ninject;
using RestSharp;
using WpfWebApiExperiment.WebApi;

namespace WpfWebApiExperiment.WebApiClient
{
    public class ApiClient : IApiClient
    {
        private readonly RestClient _restClient;

        [Inject]
        public ApiClient(RestClient restClient)
        {
            _restClient = restClient;
        }

        public List<NoteDTO> GetNotes()
        {
            var request = new RestRequest("/Notes", Method.GET);
            var response = _restClient.Execute<List<NoteDTO>>(request);
            return response.Data;
        }

        public NoteDTO GetNote(string id)
        {
            var request = new RestRequest("/Notes/{id}", Method.GET);
            request.AddParameter("id", id);

            var response = _restClient.Execute<NoteDTO>(request);
            return response.Data;
        }
    }
}
