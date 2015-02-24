using Caliburn.Micro;
using Ninject;
using WpfWebApiExperiment.Services;
using WpfWebApiExperiment.ViewModels.NoteScreen;
using WpfWebApiExperiment.WebApiClient;

namespace WpfWebApiExperiment.ViewModels
{
    public class NoteScreenViewModel : Screen
    {
        private readonly IApiExecutor _apiExecutor;
        private readonly INavigationService _navigationService;

        [Inject]
        public NoteScreenViewModel(
            IApiExecutor apiExecutor, 
            INavigationService navigationService,
            string noteId)
        {
            _apiExecutor = apiExecutor;
            _navigationService = navigationService;
            _state = new DefaultNoteScreenViewModelState(noteId);
        }

        protected override async void OnActivate()
        {
            var newState = await _state.HandleScreenActivated(_apiExecutor);
            if (newState != null)
            {
                State = newState;
            }
        }

        public void GoBack()
        {
            var newState = _state.HandleGoBack(_navigationService);
            if (newState != null)
            {
                State = newState;
            }
        }

        private INoteScreenViewModelState _state;
        public INoteScreenViewModelState State
        {
            get { return _state; }
            set
            {
                if (Equals(value, _state)) return;
                _state = value;
                NotifyOfPropertyChange(() => State);
            }
        }
    }
}