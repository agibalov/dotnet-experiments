using System.Threading.Tasks;
using WpfWebApiExperiment.WebApiClient;

namespace WpfWebApiExperiment.ViewModels.NoteScreen
{
    public class DefaultNoteScreenViewModelState : NoteScreenViewModelStateBase
    {
        private readonly string _noteId;

        public DefaultNoteScreenViewModelState(string noteId)
        {
            _noteId = noteId;
        }

        public override async Task<INoteScreenViewModelState> HandleScreenActivated(IApiExecutor apiExecutor)
        {
            var note = await apiExecutor.Execute(new GetNoteApiRequest { Id = _noteId });
            return new OkNoteScreenViewModelState(note);
        }
    }
}