using System;
using System.Threading;

namespace WpfWebApiExperiment.WebApiServer
{
    public class WebAppRunner
    {
        private ManualResetEvent _stopManualResetEvent;
        private Thread _workerThread;

        public void Start()
        {
            if (_stopManualResetEvent != null)
            {
                throw new InvalidOperationException("WebApiRunner is already running");
            }

            _stopManualResetEvent = new ManualResetEvent(false);
            _workerThread = new Thread(() =>
            {
                const string baseUrl = "http://localhost:8080/";
                using (Microsoft.Owin.Hosting.WebApp.Start<WebAppConfigurer>(baseUrl))
                {
                    _stopManualResetEvent.WaitOne();
                }
            });
            _workerThread.Start();
        }

        public void Stop()
        {
            if (_stopManualResetEvent == null)
            {
                throw new InvalidOperationException("WebApiRunner is not running");
            }

            _stopManualResetEvent.Set();
            _stopManualResetEvent = null;
            _workerThread = null;
        }
    }
}