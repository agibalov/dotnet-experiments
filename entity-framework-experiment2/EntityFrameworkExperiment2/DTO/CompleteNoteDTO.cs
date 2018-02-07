using System.Collections.Generic;

namespace EntityFrameworkExperiment2.DTO
{
    public class CompleteNoteDTO
    {
        public int NoteId { get; set; }
        public string NoteText { get; set; }
        public IList<string> Tags { get; set; }
    }
}