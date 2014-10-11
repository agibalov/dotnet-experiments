using System.Collections.ObjectModel;
using Caliburn.Micro;
using Ninject;
using WpfWebApiExperiment.WebApi;
using WpfWebApiExperiment.WebApiClient;

namespace WpfWebApiExperiment.ViewModels
{
    public class NoteListScreenViewModel : Screen
    {
        private readonly ApiClient _apiClient;
        private readonly INavigationService _navigationService;

        [Inject]
        public NoteListScreenViewModel(
            ApiClient apiClient, 
            INavigationService navigationService)
        {
            _apiClient = apiClient;
            _navigationService = navigationService;

            Notes = new ObservableCollection<NoteDTO>();
        }

        protected override async void OnActivate()
        {
            Notes.Clear();

            Message = "Loading...";

            var notes = await _apiClient.GetNotes();
            Message = string.Format("Loaded {0} notes!", notes.Count);
            
            notes.ForEach(Notes.Add);
        }

        public void ViewNote(NoteDTO note)
        {
            _navigationService.NavigateToNote(note.Id);
        }

        public ObservableCollection<NoteDTO> Notes { get; set; } 

        private string _message;
        public string Message
        {
            get { return _message; }
            set
            {
                _message = value;
                NotifyOfPropertyChange(() => Message);
            }
        }
    }
}