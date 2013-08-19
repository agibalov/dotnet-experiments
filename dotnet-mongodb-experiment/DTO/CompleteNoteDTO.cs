using System.Collections.Generic;

namespace dotnet_mongodb_experiment.DTO
{
    public class CompleteNoteDTO
    {
        public string NoteId { get; set; }
        public string Text { get; set; }
        public BriefUserDTO User { get; set; }
        public IList<BriefTagDTO> Tags { get; set; }
    }
}