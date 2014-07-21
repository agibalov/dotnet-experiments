using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TodoListApp
{
    public class MainViewModel : Screen
    {
        private readonly INavigationService _navigationService;

        public MainViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService;
        }
        public void GoToPage2()
        {
            _navigationService.NavigateToViewModel<Page2ViewModel>();
        }
    }
}
