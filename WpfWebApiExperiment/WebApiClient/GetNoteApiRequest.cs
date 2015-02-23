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
    }
}