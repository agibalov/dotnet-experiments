using ServiceStack.ServiceHost;

namespace ServiceStackWebServicesExperiment.CalculatorServiceImpl.DTO
{
    [Route("/add_numbers")]
    public class AddNumbers : IReturn<AddNumbersResult>
    {
        public int A { get; set; }
        public int B { get; set; }
    }
}