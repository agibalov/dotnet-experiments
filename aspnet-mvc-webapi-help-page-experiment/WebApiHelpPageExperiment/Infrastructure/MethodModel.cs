using System.Collections.Generic;

namespace WebApiHelpPageExperiment.Infrastructure
{
    public class MethodModel
    {
        public string Name { get; set; }
        public string HttpVerb { get; set; }
        public string RelativeUrl { get; set; }
        public string Description { get; set; }
        public IList<ParameterModel> Parameters { get; set; }
        public string ReturnSample { get; set; }
    }
}