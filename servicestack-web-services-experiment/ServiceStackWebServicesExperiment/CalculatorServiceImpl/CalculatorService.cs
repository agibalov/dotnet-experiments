using ServiceStack.ServiceInterface;
using ServiceStackWebServicesExperiment.CalculatorServiceImpl.DTO;

namespace ServiceStackWebServicesExperiment.CalculatorServiceImpl
{
    public class CalculatorService : Service
    {
        public object Any(AddNumbers addNumbers)
        {
            return new AddNumbersResult
                {
                    A = addNumbers.A,
                    B = addNumbers.B,
                    Result = addNumbers.A + addNumbers.B
                };
        }

        public object Any(SubNumbers addNumbers)
        {
            return new AddNumbersResult
                {
                    A = addNumbers.A,
                    B = addNumbers.B,
                    Result = addNumbers.A - addNumbers.B
                };
        }
    }
}