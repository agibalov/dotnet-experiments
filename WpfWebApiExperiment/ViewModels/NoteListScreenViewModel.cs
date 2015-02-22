using Caliburn.Micro;
using Ninject;
using WpfWebApiExperiment.ViewModels.NoteListScreen;
using WpfWebApiExperiment.WebApi;
using WpfWebApiExperiment.WebApiClient;

namespace WpfWebApiExperiment.ViewModels
{
    public class NoteListScreenViewModel : Screen
    {
        private readonly IApiClient _apiClient;
        private readonly INavigationService _navigationService;
        private readonly ILongOperationExecutor _longOperationExecutor;
        
        [Inject]
        public NoteListScreenViewModel(
            IApiClient apiClient, 
            INavigationService navigationService,
            ILongOperationExecutor longOperationExecutor)
        {
            _apiClient = apiClient;
            _navigationService = navigationService;
            _longOperationExecutor = longOperationExecutor;

            State = new DefaultNoteListScreenViewModelState();
        }

        protected override async void OnActivate()
        {
            var newState = await _state.HandleScreenActivated(_apiClient, _longOperationExecutor);
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
