namespace TypeSystemExperiment.Types
{
    public class VariableLvalueExpressionOfTypeParameterDefinition : IParameterDefinition
    {
        private readonly Class _clazz;

        public VariableLvalueExpressionOfTypeParameterDefinition(Class clazz)
        {
            _clazz = clazz;
        }

        public ArgumentMatchResult MatchArgument(TypeSystem typeSystem, IExpression argumentExpression)
        {
            if (!(argumentExpression is VariableLvalueExpression))
            {
                return null;
            }

            var variableExpression = (VariableLvalueExpression) argumentExpression;
            if (variableExpression.GetClass() == _clazz)
            {
                return new ArgumentMatchResult
                    {
                        Expression = argumentExpression,
                        Score = 0
                    };
            }

            return null;
        }
    }
}