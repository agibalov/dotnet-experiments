using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.ServiceProcess;
using NLog;
using Ninject;
using SelfManageableWindowsServiceExperiment.CommandHandlers;
using SelfManageableWindowsServiceExperiment.Infrastructure;
using SelfManageableWindowsServiceExperiment.Service;

namespace SelfManageableWindowsServiceExperiment
{
    class Program
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        static void Main(string[] args)
        {
            try
            {
                if (Environment.UserInteractive)
                {
                    RunAsConsole(args);
                    return;
                }

                RunAsWindowsService(args);
            }
            catch (Exception e)
            {
                Logger.FatalException("Unexpected error ocurred", e);
            }
        }

        private static void RunAsConsole(string[] args)
        {
            var commandHandlerTypes = new Dictionary<string, Type>
                {
                    {"status", typeof(PrintStatusCommandHandler)},
                    {"install", typeof(InstallCommandHandler)},
                    {"uninstall", typeof(UninstallCommandHandler)},
                    {"start", typeof(StartCommandHandler)},
                    {"stop", typeof(StopCommandHandler)},
                    {"install-and-start", typeof(InstallAndStartCommandHandler)},
                    {"stop-and-uninstall", typeof(StopAndUninstallCommandHandler)}
                };

            var command = args.Length == 1 ? args.Single() : "help";
            Type commandHandlerType;
            if (!commandHandlerTypes.TryGetValue(command, out commandHandlerType))
            {
                Console.WriteLine("Use one of these commands:");
                var listOfKnownCommands = commandHandlerTypes
                    .Keys
                    .Select(c => string.Format("  {0}", c))
                    .ToList();
                Console.WriteLine(string.Join("\n", listOfKnownCommands));
                return;
            }

            var kernel = new StandardKernel();

            var configuration = Configuration.LoadFromAppConfig();
            kernel.Bind<WindowsServiceManager>()
                .ToSelf()
                .InSingletonScope()
                .WithConstructorArgument("serviceName", configuration.WindowsServiceName)
                .WithConstructorArgument("serviceDisplayName", configuration.WindowsServiceDisplayName)
                .WithConstructorArgument("executablePath", Assembly.GetExecutingAssembly().Location);

            var commandHandler = (ICommandHandler)kernel.Get(commandHandlerType);
            commandHandler.Handle();
        }

        private static void RunAsWindowsService(string[] args)
        {
            var kernel = new StandardKernel();

            var configuration = Configuration.LoadFromAppConfig();
            kernel.Bind<DummyWindowsService>()
                .ToSelf()
                .InSingletonScope()
                .WithConstructorArgument("logAppendPeriodInSeconds", configuration.WindowsServiceLogAppendPeriodInSeconds);

            var dummyWindowsService = kernel.Get<DummyWindowsService>();
            
            ServiceBase.Run(dummyWindowsService);
        }
    }
}
