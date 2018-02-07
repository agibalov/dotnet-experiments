using ServiceStack.ServiceHost;

namespace ServiceStackWebServicesExperiment.CalculatorServiceImpl.DTO
{
    [Route("/sub_numbers")]
    public class SubNumbers : IReturn<SubNumbersResult>
    {
        public int A { get; set; }
        public int B { get; set; }
    }
}