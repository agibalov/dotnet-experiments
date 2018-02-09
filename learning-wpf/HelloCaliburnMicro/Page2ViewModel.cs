using Caliburn.Micro;

namespace HelloCaliburnMicro
{
    public class Page2ViewModel : Screen
    {
        private readonly ShellViewModel _shellViewModel;

        public Page2ViewModel(ShellViewModel shellViewModel)
        {
            _shellViewModel = shellViewModel;
        }

        public void GoPage1()
        {
            _shellViewModel.GoPage1();
        }
    }
}