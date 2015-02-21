using Caliburn.Micro;
using Ninject;
using WpfWebApiExperiment.WebApiClient;

namespace WpfWebApiExperiment.ViewModels
{
    public class NoteScreenViewModel : Screen
    {
        private readonly IApiClient _apiClient;
        private readonly INavigationService _navigationService;
        private readonly ILongOperationExecutor _longOperationExecutor;
        private readonly string _noteId;

        [Inject]
        public NoteScreenViewModel(
            IApiClient apiClient, 
            INavigationService navigationService, 
            ILongOperationExecutor longOperationExecutor,
            string noteId)
        {
            _apiClient = apiClient;
            _navigationService = navigationService;
            _longOperationExecutor = longOperationExecutor;
            _noteId = noteId;
        }

        protected override async void OnActivate()
        {
            var note = await _longOperationExecutor.Execute(() => _apiClient.GetNote(_noteId));

            Id = note.Id;
            Title = note.Title;
            Text = note.Text;
        }

        public void GoBack()
        {
            _navigationService.NavigateToNoteList();
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