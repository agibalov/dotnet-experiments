using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Caliburn.Micro;
using Ninject;
using WpfWebApiExperiment.WebApi;
using WpfWebApiExperiment.WebApiClient;

namespace WpfWebApiExperiment.ViewModels
{
    public class NoteListScreenViewModel : Screen
    {
        private readonly IApiClient _apiClient;
        private readonly INavigationService _navigationService;
        private readonly ILongOperationExecutor _longOperationExecutor;

        [Inject]
        public NoteListScreenViewModel(
            IApiClient apiClient, 
            INavigationService navigationService,
            ILongOperationExecutor longOperationExecutor)
        {
            _apiClient = apiClient;
            _navigationService = navigationService;
            _longOperationExecutor = longOperationExecutor;

            Notes = new ObservableCollection<NoteDTO>();
        }

        protected override async void OnActivate()
        {
            Notes.Clear();
            List<NoteDTO> notes = null;
            try
            {
                notes = await _longOperationExecutor.Execute(() => _apiClient.GetNotes());
            }
            catch (ApiException e) // "internal server error" or "not found"
            {
                ErrorMessage = "ApiException error";
            }
            catch (ApiClientException e) // connectivity error
            {
                ErrorMessage = "ApiClientException error";
            }
            catch (Exception e) // something very weird
            {
                ErrorMessage = "Absolutely unexpected error";
            }
            finally
            {
                if (notes != null)
                {
                    notes.ForEach(Notes.Add);
                }
            }
        }

        public void ViewNote(NoteDTO note)
        {
            _navigationService.NavigateToNote(note.Id);
        }

        public ObservableCollection<NoteDTO> Notes { get; set; }

        private string _errorMessage;
        public string ErrorMessage
        {
            get { return _errorMessage; }
            set
            {
                if (value == _errorMessage) return;
                _errorMessage = value;
                NotifyOfPropertyChange(() => ErrorMessage);
            }
        }
    }
}
