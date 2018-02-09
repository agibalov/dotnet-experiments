using System.Threading.Tasks;

namespace WpfWebApiExperiment.WebApiClient
{
    public interface IApiExecutor
    {
        Task<TResponse> Execute<TResponse>(IApiRequest<TResponse> request) 
            where TResponse : new();
    }
}