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
            catch (ApiException e) // "internal server error" or "not found"
            {
                return new ErrorNoteListScreenViewModelState("ApiException error");
            }
            catch (ApiClientException e) // connectivity error
            {
                return new ErrorNoteListScreenViewModelState("ApiClientException error");
            }
            catch (Exception e) // something very weird
            {
                return new ErrorNoteListScreenViewModelState("Absolutely unexpected error");
            }
        }
    }
}