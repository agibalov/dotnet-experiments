using System.Collections.Generic;
using System.Net;
using System.Threading;
using System.Web.Http;
using WpfWebApiExperiment.WebApi;

namespace WpfWebApiExperiment.WebApiServer
{
    public class NotesController : ApiController
    {
        private readonly NoteRepository _noteRepository;

        public NotesController(NoteRepository noteRepository)
        {
            _noteRepository = noteRepository;
        }

        public IList<NoteDTO> Get()
        {
            Thread.Sleep(2000);

            return _noteRepository.FindAll();
        }

        public NoteDTO Get(string id)
        {
            Thread.Sleep(2000);

            var note = _noteRepository.FindOne(id);
            if (note == null)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }

            return note;
        }
    }
}