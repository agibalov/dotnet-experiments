using WpfWebApiExperiment.Services;

namespace WpfWebApiExperiment.ViewModels.NoteScreen
{
    public class ErrorNoteScreenViewModelState : NoteScreenViewModelStateBase
    {
        public ErrorNoteScreenViewModelState(string errorMessage)
        {
            _errorMessage = errorMessage;
        }

        public override INoteScreenViewModelState HandleGoBack(INavigationService navigationService)
        {
            navigationService.NavigateToNoteList();
            return null;
        }

        private string _errorMessage;
        public string ErrorMessage
        {
            get { return _errorMessage; }
            set
            {
                if (value == _errorMessage) return;
                _errorMessage = value;
                OnPropertyChanged();
            }
        }
    }
}
