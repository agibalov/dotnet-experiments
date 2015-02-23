using RestSharp;

namespace WpfWebApiExperiment.WebApiClient
{
    public interface IApiClient
    {
        TResult Execute<TResult>(IRestRequest restRequest)
            where TResult : new();
    }
}
