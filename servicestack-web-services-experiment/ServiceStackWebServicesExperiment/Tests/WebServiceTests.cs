using NUnit.Framework;

namespace ServiceStackWebServicesExperiment.Tests
{
    public abstract class WebServiceTests
    {
        private const int AppHostPort = 1337;
        private readonly string _appHostUrl = string.Format("http://*:{0}/", AppHostPort);
        protected readonly string ServiceUrl = string.Format("http://localhost:{0}", AppHostPort);
        private AppHost _appHost;

        [SetUp]
        public void StartService()
        {
            _appHost = new AppHost();
            _appHost.Init();
            _appHost.Start(_appHostUrl);
        }

        [TearDown]
        public void StopService()
        {
            _appHost.Stop();
            _appHost.Dispose();
        }
    }
}