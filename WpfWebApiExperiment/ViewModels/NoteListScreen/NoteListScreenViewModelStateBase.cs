using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using WpfWebApiExperiment.Annotations;
using WpfWebApiExperiment.Services;
using WpfWebApiExperiment.WebApi;
using WpfWebApiExperiment.WebApiClient;

namespace WpfWebApiExperiment.ViewModels.NoteListScreen
{
    public abstract class NoteListScreenViewModelStateBase : INoteListScreenViewModelState, INotifyPropertyChanged
    {
        public virtual Task<INoteListScreenViewModelState> HandleScreenActivated(IApiClient apiClient, ILongOperationExecutor longOperationExecutor)
        {
            throw new InvalidOperationException();
        }

        public virtual INoteListScreenViewModelState HandleViewNote(NoteDTO note, INavigationService navigationService)
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