using Caliburn.Micro;
using Ninject;

namespace WpfWebApiExperiment.ViewModels
{
    public class ShellViewModel : Conductor<object>
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
            ActivateItem(_kernel.Get<NoteListViewModel>());
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
    }
}