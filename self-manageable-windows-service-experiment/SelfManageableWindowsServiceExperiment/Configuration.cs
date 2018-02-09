using System;
using System.Configuration;

namespace SelfManageableWindowsServiceExperiment
{
    public class Configuration
    {
        public string WindowsServiceName { get; private set; }
        public string WindowsServiceDisplayName { get; private set; }
        public int WindowsServiceLogAppendPeriodInSeconds { get; private set; }

        public static Configuration LoadFromAppConfig()
        {
            return new Configuration
                {
                    WindowsServiceName = ConfigurationManager.AppSettings["WindowsServiceName"],
                    WindowsServiceDisplayName = ConfigurationManager.AppSettings["WindowsServiceDisplayName"],
                    WindowsServiceLogAppendPeriodInSeconds = Convert.ToInt32(ConfigurationManager.AppSettings["WindowsServiceLogAppendPeriodInSeconds"])
                };
        }
    }
}