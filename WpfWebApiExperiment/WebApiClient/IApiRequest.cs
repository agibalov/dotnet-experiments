using RestSharp;

namespace WpfWebApiExperiment.WebApiClient
{
    public interface IApiRequest<TResponse>
        where TResponse : new()
    {
        IRestRequest MakeRequest();
    }
}