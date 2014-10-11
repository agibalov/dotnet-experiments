using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web.Http;
using WpfWebApiExperiment.WebApi;

namespace WpfWebApiExperiment.WebApiServer
{
    public class NotesController : ApiController
    {
        public IList<NoteDTO> Get()
        {
            Thread.Sleep(2000);

            return Enumerable.Range(1, 7).Select(i => new NoteDTO
            {
                Title = string.Format("Note #{0}", i),
                Text = string.Format("Text for note #{0} goes here", i)
            }).ToList();
        }
    }
}