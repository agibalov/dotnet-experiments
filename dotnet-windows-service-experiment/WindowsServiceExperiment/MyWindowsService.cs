using System;
using System.ServiceProcess;

namespace WindowsServiceExperiment
{
    public class MyWindowsService : ServiceBase
    {
        static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                Run(new MyWindowsService());
            }
            else if (args.Length == 1)
            {
                var command = args[0];

                var serviceManager = new ServiceManager(
                    Configuration.ServiceName,
                    typeof(MyWindowsService).Assembly);

                if (command == "-install")
                {
                    if (serviceManager.IsInstalled())
                    {
                        throw new Exception("Already installed");
                    }

                    serviceManager.Install();
                    serviceManager.Start();
                }
                else if (command == "-uninstall")
                {
                    if (!serviceManager.IsInstalled())
                    {
                        throw new Exception("Not installed");
                    }

                    if (serviceManager.IsRunning())
                    {
                        serviceManager.Stop();
                    }

                    serviceManager.Uninstall();
                }
                else
                {
                    throw new Exception("Unknown command");
                }
            }
            else
            {
                throw new Exception("0 or 1 parameter expected");
            }
        }

        public MyWindowsService()
        {
            ServiceName = Configuration.ServiceName;
            EventLog.Log = "Application";
        }

        protected override void OnStart(string[] args)
        {
            EventLog.WriteEntry("Started");
        }

        protected override void OnStop()
        {
            EventLog.WriteEntry("Stopped");
        }
    }
}