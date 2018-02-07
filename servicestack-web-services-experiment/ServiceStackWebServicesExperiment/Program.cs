using System;

namespace ServiceStackWebServicesExperiment
{
    class Program
    {
        static void Main(string[] args)
        {
            SelfHostServices();
        }

        private static void SelfHostServices()
        {
            using (var appHost = new AppHost())
            {
                appHost.Init();
                appHost.Start("http://*:1337/");

                Console.WriteLine("Listening at http://localhost:1337/");
                Console.WriteLine("Press any key to exit");
                Console.ReadKey();

                appHost.Stop();
            }
        }
    }
}
