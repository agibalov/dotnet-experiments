using System;
using System.Collections.Generic;
using System.Linq;
using WpfWebApiExperiment.WebApi;

namespace WpfWebApiExperiment.WebApiServer
{
    public class NoteRepository
    {
        private readonly List<NoteDTO> _notes = new List<NoteDTO>();

        public NoteRepository()
        {
            var notes = Enumerable.Range(1, 7).Select(i => new NoteDTO
            {
                Title = string.Format("Note #{0}", i),
                Text = string.Format("Text for note #{0} goes here", i)
            });

            foreach (var note in notes)
            {
                Save(note);
            }
        }

        public NoteDTO Save(NoteDTO note)
        {
            if (note.Id != null)
            {
                var existingNoteId = note.Id;
                var existingNote = FindOne(existingNoteId);
                if (existingNote == null)
                {
                    throw new InvalidOperationException("There's no note with ID: " + existingNoteId);
                }

                existingNote.Title = note.Title;
                existingNote.Text = note.Text;
            }
            else
            {
                note.Id = Guid.NewGuid().ToString();
                _notes.Add(note);
            }

            return note;
        }

        public List<NoteDTO> FindAll()
        {
            return new List<NoteDTO>(_notes);
        }

        public NoteDTO FindOne(string id)
        {
            return _notes.SingleOrDefault(note => note.Id == id);
        }
    }
}