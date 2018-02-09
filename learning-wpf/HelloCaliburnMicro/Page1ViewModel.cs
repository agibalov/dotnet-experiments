using System.Windows;
using Caliburn.Micro;

namespace HelloCaliburnMicro
{
    public class Page1ViewModel : Screen
    {
        private readonly KindOfRemoteService _kindOfRemoteService;
        private readonly ShellViewModel _shellViewModel;

        public Page1ViewModel(KindOfRemoteService kindOfRemoteService, ShellViewModel shellViewModel)
        {
            _kindOfRemoteService = kindOfRemoteService;
            _shellViewModel = shellViewModel;
        }

        protected override async void OnActivate()
        {
            Name = "loading... " + _kindOfRemoteService;
            Name = await _kindOfRemoteService.GetDefaultValue();
        }

        private string _name;

        public string Name
        {
            get { return _name; }
            set
            {
                _name = value;
                NotifyOfPropertyChange(() => Name);
            }
        }

        public async void SayHello()
        {
            var message = await _kindOfRemoteService.MakeHelloMessage(Name);
            MessageBox.Show(message);
        }

        public void GoPage2()
        {
            _shellViewModel.GoPage2();
        }
    }
}