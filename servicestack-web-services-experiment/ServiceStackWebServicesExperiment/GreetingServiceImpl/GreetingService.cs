using ServiceStack.ServiceInterface;
using ServiceStackWebServicesExperiment.GreetingServiceImpl.DTO;

namespace ServiceStackWebServicesExperiment.GreetingServiceImpl
{
    public class GreetingService : Service
    {
        public object Any(GreetMe greetMe)
        {
            return new GreetMeResponseDTO
                {
                    Message = string.Format("Hello {0}", greetMe.Name)
                };
        }

        public object Any(UngreetMe ungreetMe)
        {
            return new UngreetMeResponseDTO
                {
                    Message = string.Format("Bye {0}", ungreetMe.Name)
                };
        }
    }
}