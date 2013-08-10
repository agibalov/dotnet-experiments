using System;
using Nancy.Hosting.Self;

namespace NancyExperiment
{
    class Program
    {
        static void Main(string[] args)
        {
            var nancyHost = new NancyHost(new Uri("http://localhost:2302"));
            nancyHost.Start();

            Console.WriteLine("Listening at http://localhost:2302");
            Console.WriteLine("Press any key to quit");

            Console.ReadKey();
            nancyHost.Stop();
        }
    }
}
