using System.Collections.Generic;
using WpfWebApiExperiment.WebApi;

namespace WpfWebApiExperiment.WebApiClient
{
    public interface IApiClient
    {
        List<NoteDTO> GetNotes();
        NoteDTO GetNote(string id);
    }
}
