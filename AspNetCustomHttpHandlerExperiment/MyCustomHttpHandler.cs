using System;
using System.Linq;
using System.Web;
using Newtonsoft.Json;

namespace AspNetCustomHttpHandlerExperiment
{
    public class MyCustomHttpHandler : IHttpHandler
    {
        public void ProcessRequest(HttpContext context)
        {
            var requestedActionName = context.Request.Url.Segments.Last();

            if (string.Equals(requestedActionName, "text", StringComparison.InvariantCultureIgnoreCase))
            {
                context.Response.ContentType = "text/plain";
                context.Response.StatusCode = 200;
                context.Response.Write("Hello there. It's just a plain text");
            }
            else if (string.Equals(requestedActionName, "json", StringComparison.InvariantCultureIgnoreCase))
            {
                var obj = new
                    {
                        Id = 123,
                        Message = "Hello there. It's just a JSON"
                    };

                var objJson = JsonConvert.SerializeObject(obj);

                context.Response.ContentType = "application/json";
                context.Response.StatusCode = 200;
                context.Response.Write(objJson);
            }
            else if (string.Equals(requestedActionName, "html", StringComparison.InvariantCultureIgnoreCase))
            {
                context.Response.ContentType = "text/html";
                context.Response.StatusCode = 200;
                context.Response.Write(
@"<html>
    <head>
        <title>Custom HTTP Handler Experiment</title>
    </head>
    <body>
        <h1>HTML response</h1>        
    </body>
</html>");
            }
            else
            {
                context.Response.ContentType = "text/html";
                context.Response.StatusCode = 200;
                context.Response.Write(
@"<html>
    <head>
        <title>Custom HTTP Handler Experiment</title>
    </head>
    <body>
        <h1>Custom HTTP Handler Experiment</h1>
        <ul>
            <li><a href=""AspNetCustomHttpHandlerExperiment/text"">Plain Text Response</a></li>
            <li><a href=""AspNetCustomHttpHandlerExperiment/json"">JSON Response</a></li>
            <li><a href=""AspNetCustomHttpHandlerExperiment/html"">HTML Response</a></li>
        </ul>
    </body>
</html>");
            }
        }

        public bool IsReusable
        {
            get { return false; }
        }
    }
}