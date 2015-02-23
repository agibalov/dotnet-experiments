using System.Threading.Tasks;
using Ninject;
using WpfWebApiExperiment.Services;

namespace WpfWebApiExperiment.WebApiClient
{
    public class ApiExecutor : IApiExecutor
    {
        private readonly ILongOperationExecutor _longOperationExecutor;
        private readonly ApiClient _apiClient;

        [Inject]
        public ApiExecutor(ILongOperationExecutor longOperationExecutor, ApiClient apiClient)
        {
            _longOperationExecutor = longOperationExecutor;
            _apiClient = apiClient;
        }

        public async Task<TResponse> Execute<TResponse>(IApiRequest<TResponse> request)
            where TResponse : new()
        {
            return await _longOperationExecutor.Execute(() =>
            {
                var restRequest = request.MakeRequest();
                var result = _apiClient.Execute<TResponse>(restRequest);
                return result;
            });
        }
    }
}