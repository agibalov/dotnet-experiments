using WpfWebApiExperiment.Services;
using WpfWebApiExperiment.WebApi;

namespace WpfWebApiExperiment.ViewModels.NoteScreen
{
    public class OkNoteScreenViewModelState : NoteScreenViewModelStateBase
    {
        public OkNoteScreenViewModelState(NoteDTO note)
        {
            Id = note.Id;
            Title = note.Title;
            Text = note.Text;
        }

        public override INoteScreenViewModelState HandleGoBack(INavigationService navigationService)
        {
            navigationService.NavigateToNoteList();
            return null;
        }

        private string _id;
        public string Id
        {
            get { return _id; }
            set
            {
                _id = value;
                OnPropertyChanged();
            }
        }

        private string _title;
        public string Title
        {
            get { return _title; }
            set
            {
                _title = value;
                OnPropertyChanged();
            }
        }

        private string _text;
        public string Text
        {
            get { return _text; }
            set
            {
                _text = value;
                OnPropertyChanged();
            }
        }
    }
}