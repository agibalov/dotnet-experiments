using System;
using System.Windows.Controls;
using Ninject;

namespace HelloArchitecture
{
    public class MyNavService : INavService
    {
        private readonly Frame _frame;
        private readonly IKernel _kernel;

        public MyNavService(Frame frame, IKernel kernel)
        {
            _frame = frame;
            _kernel = kernel;
        }

        public void NavigateTo(Type pageType, object parameter)
        {
            var page = (BasePage)_kernel.Get(pageType);
            page.NavParameter = parameter;
            page.DataContext = _kernel.Get<object>(pageType.Name + "viewmodel");
            _frame.Navigate(page);
        }
    }
}