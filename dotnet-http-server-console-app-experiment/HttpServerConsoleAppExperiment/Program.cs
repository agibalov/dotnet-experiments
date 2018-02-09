using System;
using System.Net;
using System.Text;

namespace HttpServerConsoleAppExperiment
{
    class Program
    {
        static void Main(string[] args)
        {
            var listener = new HttpListener();
            listener.Prefixes.Add("http://+:8081/");
            listener.Start();

            for (var requestCount = 1; ; ++requestCount)
            {
                var context = listener.GetContext();
                var request = context.Request;

                Console.WriteLine("{0} - {1} - {2}",
                    DateTime.Now,
                    request.RemoteEndPoint.Address,
                    request.Url);

                var responseString = string.Format(@"
<html>
    <head>
        <title>The Web Server</title>
    </head>
    <body>
        <h1>Hi {0}!</h1>
        <p>Here's what you're requesting: {1}</p>
        <p>It's request #{2} processed by this server.</p>
    </body>
</html>
                ", 
                 request.RemoteEndPoint.Address,
                 request.Url, 
                 requestCount);

                var responseBytes = Encoding.UTF8.GetBytes(responseString);

                context.Response.OutputStream.Write(responseBytes, 0, responseBytes.Length);
                context.Response.Close();
            }
        }
    }
}
