using System.Collections.Generic;
using Newtonsoft.Json;

namespace EntityFrameworkExperiment2.DAL.Entities
{
    public class Note
    {
        public int NoteId { get; set; }
        public string NoteText { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
        public IList<Tag> Tags { get; set; }

        public override string ToString()
        {
            return string.Format("Note {0}", JsonConvert.SerializeObject(this, Formatting.Indented));
        }
    }
}