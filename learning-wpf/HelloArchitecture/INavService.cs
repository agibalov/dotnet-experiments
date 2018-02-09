using System;

namespace HelloArchitecture
{
    public interface INavService
    {
        void NavigateTo(Type pageType, object parameter);
    }
}