using Caliburn.Micro;
using Ninject;
using WpfWebApiExperiment.Services;
using WpfWebApiExperiment.ViewModels.NoteListScreen;
using WpfWebApiExperiment.WebApi;
using WpfWebApiExperiment.WebApiClient;

namespace WpfWebApiExperiment.ViewModels
{
    public class NoteListScreenViewModel : Screen
    {
        private readonly IApiExecutor _apiExecutor;
        private readonly INavigationService _navigationService;
        
        [Inject]
        public NoteListScreenViewModel(
            IApiExecutor apiExecutor,
            INavigationService navigationService)
        {
            _apiExecutor = apiExecutor;
            _navigationService = navigationService;

            State = new DefaultNoteListScreenViewModelState();
        }

        protected override async void OnActivate()
        {
            var newState = await _state.HandleScreenActivated(_apiExecutor);
            if (newState != null)
            {
                State = newState;
            }
        }

        public void ViewNote(NoteDTO note)
        {
            var newState = State.HandleViewNote(note, _navigationService);
            if (newState != null)
            {
                State = newState;
            }
        }

        private INoteListScreenViewModelState _state;
        public INoteListScreenViewModelState State
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
