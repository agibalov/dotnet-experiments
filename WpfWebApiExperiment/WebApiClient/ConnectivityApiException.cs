namespace WpfWebApiExperiment.WebApiClient
{
    public class ConnectivityApiException : ApiException
    {
        public ConnectivityApiException(string message)
            : base(message)
        {
        }
    }
}