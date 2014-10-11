using Caliburn.Micro;
using Ninject;
using WpfWebApiExperiment.WebApiClient;

namespace WpfWebApiExperiment.ViewModels
{
    public class NoteScreenViewModel : Screen
    {
        private readonly ApiClient _apiClient;
        private readonly INavigationService _navigationService;
        private readonly string _noteId;

        [Inject]
        public NoteScreenViewModel(
            ApiClient apiClient, 
            INavigationService navigationService, 
            string noteId)
        {
            _apiClient = apiClient;
            _navigationService = navigationService;
            _noteId = noteId;
        }

        protected override async void OnActivate()
        {
            Message = "Loading...";

            var note = await _apiClient.GetNote(_noteId);
            Message = string.Format("Loaded {0}!", _noteId);

            Id = note.Id;
            Title = note.Title;
            Text = note.Text;
        }

        public void GoBack()
        {
            _navigationService.NavigateToNoteList();
        }

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

        private string _id;
        public string Id
        {
            get { return _id; }
            set
            {
                _id = value;
                NotifyOfPropertyChange(() => Id);
            }
        }

        private string _title;
        public string Title
        {
            get { return _title; }
            set
            {
                _title = value;
                NotifyOfPropertyChange(() => Title);
            }
        }

        private string _text;
        public string Text
        {
            get { return _text; }
            set
            {
                _text = value;
                NotifyOfPropertyChange(() => Text);
            }
        }
    }
}