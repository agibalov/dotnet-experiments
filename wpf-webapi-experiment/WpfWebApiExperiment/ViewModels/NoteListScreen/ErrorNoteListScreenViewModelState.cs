namespace WpfWebApiExperiment.ViewModels.NoteListScreen
{
    public class ErrorNoteListScreenViewModelState : NoteListScreenViewModelStateBase
    {
        public ErrorNoteListScreenViewModelState(string message)
        {
            ErrorMessage = message;
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