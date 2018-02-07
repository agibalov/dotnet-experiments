using ServiceStack.ServiceHost;

namespace ServiceStackWebServicesExperiment.GreetingServiceImpl.DTO
{
    [Route("/hello")]
    public class GreetMe : IReturn<GreetMeResponseDTO>
    {
        public string Name { get; set; }
    }
}