using Caliburn.Micro;
using Ninject;
using WpfWebApiExperiment.Services;
using WpfWebApiExperiment.WebApiClient;

namespace WpfWebApiExperiment.ViewModels
{
    public class NoteScreenViewModel : Screen
    {
        private readonly IApiExecutor _apiExecutor;
        private readonly INavigationService _navigationService;
        private readonly string _noteId;

        [Inject]
        public NoteScreenViewModel(
            IApiExecutor apiExecutor, 
            INavigationService navigationService,
            string noteId)
        {
            _apiExecutor = apiExecutor;
            _navigationService = navigationService;
            _noteId = noteId;
        }

        protected override async void OnActivate()
        {
            var note = await _apiExecutor.Execute(new GetNoteApiRequest {Id = _noteId});

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