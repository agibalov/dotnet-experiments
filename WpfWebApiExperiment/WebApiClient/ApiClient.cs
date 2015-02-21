using System.Collections.Generic;
using System.Threading.Tasks;
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

        public async Task<List<NoteDTO>> GetNotes()
        {
            var request = new RestRequest("/Notes", Method.GET);
            var response = await _restClient.ExecuteTaskAsync<List<NoteDTO>>(request);
            return response.Data;
        }

        public async Task<NoteDTO> GetNote(string id)
        {
            var request = new RestRequest("/Notes/{id}", Method.GET);
            request.AddParameter("id", id);

            var response = await _restClient.ExecuteTaskAsync<NoteDTO>(request);
            return response.Data;
        }
    }
}
