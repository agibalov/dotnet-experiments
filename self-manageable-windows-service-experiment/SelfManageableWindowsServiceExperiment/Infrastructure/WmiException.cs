using System;

namespace SelfManageableWindowsServiceExperiment.Infrastructure
{
    public class WmiException : Exception
    {
        public int WmiErrorCode { get; private set; }

        public WmiException(int wmiErrorCode)
            : base(string.Format("WMI method call returned {0}", wmiErrorCode))
        {
            WmiErrorCode = wmiErrorCode;
        }
    }
}