using System.Threading.Tasks;
using WpfWebApiExperiment.Services;
using WpfWebApiExperiment.WebApi;
using WpfWebApiExperiment.WebApiClient;

namespace WpfWebApiExperiment.ViewModels.NoteListScreen
{
    public interface INoteListScreenViewModelState
    {
        Task<INoteListScreenViewModelState> HandleScreenActivated(IApiExecutor apiExecutor);
        INoteListScreenViewModelState HandleViewNote(NoteDTO note, INavigationService navigationService);
    }
}