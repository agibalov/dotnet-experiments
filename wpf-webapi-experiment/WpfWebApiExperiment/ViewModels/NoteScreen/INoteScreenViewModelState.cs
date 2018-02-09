using System.Threading.Tasks;
using WpfWebApiExperiment.Services;
using WpfWebApiExperiment.WebApiClient;

namespace WpfWebApiExperiment.ViewModels.NoteScreen
{
    public interface INoteScreenViewModelState
    {
        Task<INoteScreenViewModelState> HandleScreenActivated(IApiExecutor apiExecutor);
        INoteScreenViewModelState HandleGoBack(INavigationService navigationService);
    }
}