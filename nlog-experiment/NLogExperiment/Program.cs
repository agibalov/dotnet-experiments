using System;
using NLog;
using NLog.Config;
using NLog.Targets;
using Ninject;

namespace NLogExperiment
{
    class Program
    {
        static void Main(string[] args)
        {
            var kernel = new StandardKernel();
            ConfigurationItemFactory.Default.CreateInstance = type => kernel.TryGet(type);
            kernel.Bind<IMyService>().To<MyService>().InSingletonScope();
            kernel.Bind<Loki2302ConsoleTarget>()
                  .ToSelf()
                  .InSingletonScope()
                  .WithConstructorArgument("label", "Injected Label");

            var service = kernel.Get<IMyService>();
            Console.WriteLine(service.AddNumbers(1, 2));
            service.Exception();
        }
    }

    public interface IMyService
    {
        int AddNumbers(int a, int b);
        void Exception();
    }

    public class MyService : IMyService
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        public int AddNumbers(int a, int b)
        {
            Logger.Info("Adding {0} and {1}", a, b);
            return a + b;
        }

        public void Exception()
        {
            try
            {
                var y = 1;
                var x = 1/(y - 1);
            }
            catch (Exception e)
            {
                Logger.ErrorException("Something terrible happened while trying to divide", e);
            }
        }
    }

    [Target("Loki2302Console")]
    public class Loki2302ConsoleTarget : Target
    {
        private readonly string _label;

        public Loki2302ConsoleTarget(string label)
        {
            _label = label;
        }

        protected override void Write(LogEventInfo logEvent)
        {
            Console.WriteLine("LOKI2302[{2}]: '{0}' at {1}", logEvent.FormattedMessage, logEvent.TimeStamp, _label);
        }
    }
}
