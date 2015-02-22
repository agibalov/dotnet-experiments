using Caliburn.Micro;
using Ninject;
using Ninject.Parameters;
using WpfWebApiExperiment.ViewModels.NoteListScreen;

namespace WpfWebApiExperiment.ViewModels
{
    public class ShellViewModel : Conductor<object>, INavigationService, ILongOperationListener
    {
        private readonly IKernel _kernel;

        [Inject]
        public ShellViewModel(IKernel kernel)
        {
            _kernel = kernel;
        }

        protected override void OnActivate()
        {
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

        public void OnOperationStarted()
        {
            Message = "Loading...";
        }

        public void OnOperationFinished()
        {
            Message = "Done!";
        }
    }
}
