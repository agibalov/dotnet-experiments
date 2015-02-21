using System.Collections.Generic;
using System.Threading.Tasks;
using WpfWebApiExperiment.WebApi;

namespace WpfWebApiExperiment.WebApiClient
{
    public interface IApiClient
    {
        Task<List<NoteDTO>> GetNotes();
        Task<NoteDTO> GetNote(string id);
    }
}