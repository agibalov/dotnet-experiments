using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WpfWebApiExperiment.WebApi;
using WpfWebApiExperiment.WebApiClient;

namespace WpfWebApiExperiment.ViewModels.NoteListScreen
{
    public class DefaultNoteListScreenViewModelState : NoteListScreenViewModelStateBase
    {
        public override async Task<INoteListScreenViewModelState> HandleScreenActivated(IApiExecutor apiExecutor)
        {
            try
            {
                List<NoteDTO> notes = await apiExecutor.Execute(new GetNotesApiRequest());
                return new OkNoteListScreenViewModelState(notes);
            }
            catch (ConnectivityApiException)
            {
                return new ErrorNoteListScreenViewModelState("ConnectivityApiException error");
            }
            catch (ApiException)
            {
                return new ErrorNoteListScreenViewModelState("ApiException error");
            }
            catch (Exception)
            {
                return new ErrorNoteListScreenViewModelState("Absolutely unexpected error");
            }
        }
    }
}