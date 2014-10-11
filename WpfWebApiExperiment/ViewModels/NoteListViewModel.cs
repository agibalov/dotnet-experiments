using System.Collections.ObjectModel;
using Caliburn.Micro;
using Ninject;
using WpfWebApiExperiment.WebApi;
using WpfWebApiExperiment.WebApiClient;

namespace WpfWebApiExperiment.ViewModels
{
    public class NoteListViewModel : Screen
    {
        private readonly ApiClient _apiClient;

        [Inject]
        public NoteListViewModel(ApiClient apiClient)
        {
            _apiClient = apiClient;

            Notes = new ObservableCollection<NoteDTO>();
        }

        protected override async void OnActivate()
        {
            Message = "Loading...";

            var notes = await _apiClient.GetNotes();
            Message = string.Format("Loaded {0} notes!", notes.Count);
            
            Notes.Clear();
            notes.ForEach(Notes.Add);
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