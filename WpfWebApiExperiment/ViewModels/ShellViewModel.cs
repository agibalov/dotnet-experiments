using Caliburn.Micro;
using Ninject;
using Ninject.Parameters;

namespace WpfWebApiExperiment.ViewModels
{
    public class ShellViewModel : Conductor<object>, INavigationService
    {
        private readonly IKernel _kernel;

        [Inject]
        public ShellViewModel(IKernel kernel)
        {
            _kernel = kernel;
        }

        protected override void OnActivate()
        {
            Message = "I am ShellView";
            NavigateToNoteList();
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

        public void NavigateToNoteList()
        {
            ActivateItem(_kernel.Get<NoteListScreenViewModel>());
        }

        public void NavigateToNote(string id)
        {
            ActivateItem(_kernel.Get<NoteScreenViewModel>(new ConstructorArgument("noteId", id)));
        }
    }
}