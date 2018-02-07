using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace AspNetMvcBackgroundWorkerExperiment.Controllers
{
    public class MyApiController : ApiController
    {
        private readonly CounterService _counterService;

        public MyApiController(CounterService counterService)
        {
            _counterService = counterService;
        }

        [ActionName("GetStatus")]
        [HttpGet]
        public Status GetStatus()
        {
            var counterServiceStatus = _counterService.GetStatus();
            return new Status
                {
                    Count = counterServiceStatus.Count,
                    IsRunning = counterServiceStatus.IsRunning,
                    Log = counterServiceStatus.Log
                };
        }

        [ActionName("Start")]
        [HttpPost]
        public void Start()
        {
            _counterService.Start();
        }

        [ActionName("Stop")]
        [HttpPost]
        public void Stop()
        {
            _counterService.Stop();
        }
    }

    public class Status
    {
        public int Count { get; set; }
        public bool IsRunning { get; set; }
        public IList<string> Log { get; set; }
    }
}
