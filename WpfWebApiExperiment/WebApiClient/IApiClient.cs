namespace WpfWebApiExperiment.WebApiClient
{
    public interface IApiClient
    {
        TResult Execute<TResult>(IApiRequest<TResult> apiRequest)
            where TResult : new();
    }
}
