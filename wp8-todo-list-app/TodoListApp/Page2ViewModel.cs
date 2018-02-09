using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TodoListApp
{
    public class Page2ViewModel : Screen
    {
        private readonly INavigationService _navigationService;

        public Page2ViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService;
        }

        public void GoBack()
        {
            _navigationService.GoBack();
        }
    }
}
