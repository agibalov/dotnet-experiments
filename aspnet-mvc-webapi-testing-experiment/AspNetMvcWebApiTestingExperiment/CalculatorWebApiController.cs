using System.Web.Http;

namespace AspNetMvcWebApiTestingExperiment
{
    public class CalculatorWebApiController : ApiController
    {
        [ActionName("AddNumbers")]
        [HttpGet]
        public AddNumbersResult AddNumbers(int a, int b)
        {
            var result = new AddNumbersResult
                {
                    NumberA = a,
                    NumberB = b,
                    Result = a + b
                };

            return result;
        }
    }
}