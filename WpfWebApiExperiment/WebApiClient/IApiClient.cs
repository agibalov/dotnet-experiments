using RestSharp;

namespace WpfWebApiExperiment.WebApiClient
{
    public interface IApiClient
    {
        /*List<NoteDTO> GetNotes();
        NoteDTO GetNote(string id);*/

        TResult Execute<TResult>(IRestRequest restRequest)
            where TResult : new();
    }
}
