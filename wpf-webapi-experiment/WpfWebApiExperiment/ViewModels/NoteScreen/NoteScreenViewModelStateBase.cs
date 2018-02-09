using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using WpfWebApiExperiment.Annotations;
using WpfWebApiExperiment.Services;
using WpfWebApiExperiment.WebApiClient;

namespace WpfWebApiExperiment.ViewModels.NoteScreen
{
    public abstract class NoteScreenViewModelStateBase : INoteScreenViewModelState, INotifyPropertyChanged
    {
        public virtual Task<INoteScreenViewModelState> HandleScreenActivated(IApiExecutor apiExecutor)
        {
            throw new InvalidOperationException();
        }

        public virtual INoteScreenViewModelState HandleGoBack(INavigationService navigationService)
        {
            throw new InvalidOperationException();
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