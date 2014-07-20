using System.ComponentModel;
using System.Runtime.CompilerServices;
using HelloArchitecture.Annotations;

namespace HelloArchitecture
{
    public class MainPageViewModel : INavAware, INotifyPropertyChanged
    {
        private readonly KindOfRemoteService _kindOfRemoteService;

        public MainPageViewModel(KindOfRemoteService kindOfRemoteService)
        {
            _kindOfRemoteService = kindOfRemoteService;
        }

        private string _something;
        public string Something
        {
            get
            {
                return _something;
            }

            set
            {
                _something = value;
                OnPropertyChanged();
            }
        }

        public async void OnNavigatedTo(object parameter)
        {
            Something = "Loading...";
            Something = await _kindOfRemoteService.GetMagicValue(parameter.ToString());
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            var handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}