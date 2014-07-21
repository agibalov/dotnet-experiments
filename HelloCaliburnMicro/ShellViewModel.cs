using Caliburn.Micro;

namespace HelloCaliburnMicro
{
    public class ShellViewModel : Conductor<object>
    {
        protected override void OnActivate()
        {
            GoPage1();
        }

        public void GoPage1()
        {
            ActivateItem(IoC.Get<Page1ViewModel>());
        }

        public void GoPage2()
        {
            ActivateItem(IoC.Get<Page2ViewModel>());
        }
    }
}