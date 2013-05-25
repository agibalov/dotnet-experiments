using System.Collections.Generic;

namespace TypeSystemExperiment.Types
{
    public class CandidateOperation
    {
        public OperationDefinition OperationDefinition { get; set; }
        public int Score { get; set; }
        public IList<IExpression> Arguments { get; set; }
    }
}