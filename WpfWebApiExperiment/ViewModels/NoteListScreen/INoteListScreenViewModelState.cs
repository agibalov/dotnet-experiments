using System.Threading.Tasks;
using WpfWebApiExperiment.Services;
using WpfWebApiExperiment.WebApi;
using WpfWebApiExperiment.WebApiClient;

namespace WpfWebApiExperiment.ViewModels.NoteListScreen
{
    public interface INoteListScreenViewModelState
    {
        Task<INoteListScreenViewModelState> HandleScreenActivated(IApiClient apiClient, ILongOperationExecutor longOperationExecutor);
        INoteListScreenViewModelState HandleViewNote(NoteDTO note, INavigationService navigationService);
    }
}