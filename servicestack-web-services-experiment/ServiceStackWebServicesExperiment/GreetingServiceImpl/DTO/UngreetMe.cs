using ServiceStack.ServiceHost;

namespace ServiceStackWebServicesExperiment.GreetingServiceImpl.DTO
{
    [Route("/bye")]
    public class UngreetMe : IReturn<UngreetMeResponseDTO>
    {
        public string Name { get; set; }
    }
}