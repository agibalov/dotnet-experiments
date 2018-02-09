using System;
using Nancy.Hosting.Self;

namespace NancyExperiment
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var nancyHost = new NancyHost(new Uri("http://localhost:2302")))
            {
                nancyHost.Start();

                Console.WriteLine("Listening at http://localhost:2302");
                Console.WriteLine("Press enter to exit");

                Console.ReadLine();
            }
        }
    }
}
