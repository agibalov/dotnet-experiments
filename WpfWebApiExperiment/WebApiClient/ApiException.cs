using System;

namespace WpfWebApiExperiment.WebApiClient
{
    public class ApiException : Exception
    {
        public ApiException(string message)
            : base(message)
        {
        }
    }
}
