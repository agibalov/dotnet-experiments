using System.Collections.ObjectModel;
using Caliburn.Micro;
using Ninject;
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

            Notes = new ObservableCollection<NoteDTO>();
        }

        protected override async void OnActivate()
        {
            Notes.Clear();

            var notes = await _longOperationExecutor.Execute(() => _apiClient.GetNotes());
            
            notes.ForEach(Notes.Add);
        }

        public void ViewNote(NoteDTO note)
        {
            _navigationService.NavigateToNote(note.Id);
        }

        public ObservableCollection<NoteDTO> Notes { get; set; } 
    }
}