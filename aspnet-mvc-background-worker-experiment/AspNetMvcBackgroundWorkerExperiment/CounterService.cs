using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;

namespace AspNetMvcBackgroundWorkerExperiment
{
    public class CounterService
    {
        private Thread _workerThread;
        private int _count;
        private IList<string> _messages = new List<string>();
        private bool _shouldStop;

        public void Start()
        {
            if (_workerThread != null)
            {
                throw new Exception("Thread already running");
            }

            _workerThread = new Thread(WorkerProc);
            _shouldStop = false;
            _workerThread.Start();
        }

        public void Stop()
        {
            if (_workerThread == null)
            {
                throw new Exception("Thread is not running");
            }

            lock (typeof (CounterService))
            {
                _shouldStop = true;
            }

            Log("Waiting for thread to stop");
            _workerThread.Join();
            _workerThread = null;
            Log("Thread stopped");
        }

        public CounterServiceStatus GetStatus()
        {
            lock (typeof (CounterService))
            {
                return new CounterServiceStatus
                    {
                        Count = _count,
                        IsRunning = _workerThread != null,
                        Log = new List<string>(_messages)
                    };
            }
        }

        private void WorkerProc()
        {
            try
            {
                Log("Started");
                while (true)
                {
                    lock (typeof (CounterService))
                    {
                        if (_shouldStop)
                        {
                            return;
                        }
                    }

                    Thread.Sleep(1000);

                    lock (typeof (CounterService))
                    {
                        ++_count;
                        Log("Working - {0}", _count);
                    }
                }
            }
            finally
            {
                Log("Stopped");
            }
        }

        private void Log(string format, params object[] o)
        {
            var s = string.Format(format, o);
            s = string.Format("{0} {1}\n", DateTime.Now, s);
            lock (typeof (CounterService))
            {
                _messages.Insert(0, s);
                _messages = _messages.Take(10).ToList();
            }
        }
    }

    public class CounterServiceStatus
    {
        public int Count { get; set; }
        public bool IsRunning { get; set; }
        public IList<string> Log { get; set; }
    }
}