using System;
using System.ServiceProcess;
using System.Threading;
using NLog;

namespace SelfManageableWindowsServiceExperiment.Service
{
    public class DummyWindowsService : ServiceBase
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        private readonly int _logAppendPeriodInSeconds;
        private Thread _thread;
        private volatile bool _shouldStop;

        public DummyWindowsService(int logAppendPeriodInSeconds)
        {
            _logAppendPeriodInSeconds = logAppendPeriodInSeconds;
        }

        protected override void OnStart(string[] args)
        {
            try
            {
                if (_thread != null)
                {
                    throw new Exception("Service already running");
                }

                _shouldStop = false;
                _thread = new Thread(() =>
                    {
                        Logger.Info("Worker thread started");

                        while (!_shouldStop)
                        {
                            for (var i = 0; i < _logAppendPeriodInSeconds; ++i)
                            {
                                Thread.Sleep(1000);
                            }

                            Logger.Info("Working...");
                        }

                        Logger.Info("Worker thread finished");
                    });
                _thread.Start();
            }
            catch (Exception e)
            {
                Logger.ErrorException("Can't start", e);
            }
        }

        protected override void OnStop()
        {
            try
            {
                if (_thread == null)
                {
                    throw new Exception("Service not running");
                }

                _shouldStop = true;
                _thread.Join();
                _thread = null;
            }
            catch (Exception e)
            {
                Logger.ErrorException("Can't stop", e);
            }
        }
    }
}