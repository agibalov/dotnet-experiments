using System.Web;

namespace AspNetCustomHttpHandlerExperiment
{
    public class MyCustomHttpHandlerFactory : IHttpHandlerFactory
    {
        public IHttpHandler GetHandler(HttpContext context, string requestType, string url, string pathTranslated)
        {
            return new MyCustomHttpHandler();
        }

        public void ReleaseHandler(IHttpHandler handler)
        {
        }
    }
}