using System;
using System.Threading.Tasks;
using Caliburn.Micro;
using Ninject;
using Ninject.Parameters;

namespace WpfWebApiExperiment.ViewModels
{
    public class ShellViewModel : Conductor<object>, INavigationService, ILongOperationExecutor
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

        public async Task<T> Execute<T>(Func<Task<T>> func)
        {
            Message = "Loading...";
            try
            {
                return await func();
            }
            finally
            {
                Message = "Done!";
            }
        }
    }
}