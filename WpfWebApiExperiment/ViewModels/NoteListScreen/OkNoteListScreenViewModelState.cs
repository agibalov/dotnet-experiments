using System.Collections.Generic;
using System.Collections.ObjectModel;
using WpfWebApiExperiment.Services;
using WpfWebApiExperiment.WebApi;

namespace WpfWebApiExperiment.ViewModels.NoteListScreen
{
    public class OkNoteListScreenViewModelState : NoteListScreenViewModelStateBase
    {
        public OkNoteListScreenViewModelState(List<NoteDTO> notes)
        {
            Notes = new ObservableCollection<NoteDTO>();
            notes.ForEach(Notes.Add);
        }

        public override INoteListScreenViewModelState HandleViewNote(NoteDTO note, INavigationService navigationService)
        {
            navigationService.NavigateToNote(note.Id);
            return null;
        }

        public ObservableCollection<NoteDTO> Notes { get; set; }
    }
}