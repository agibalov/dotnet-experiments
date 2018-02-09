namespace WpfWebApiExperiment.WebApiClient
{
    public class NoteNotFoundException : ApiException
    {
        public NoteNotFoundException(string noteId) 
            : base(string.Format("Note #{0} not found", noteId))
        {
        }
    }
}