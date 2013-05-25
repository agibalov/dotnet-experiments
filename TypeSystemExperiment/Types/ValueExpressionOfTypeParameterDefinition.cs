namespace TypeSystemExperiment.Types
{
    public class ValueExpressionOfTypeParameterDefinition : IParameterDefinition
    {
        private readonly Class _clazz;

        public ValueExpressionOfTypeParameterDefinition(Class clazz)
        {
            _clazz = clazz;
        }

        public ArgumentMatchResult MatchArgument(TypeSystem typeSystem, IExpression argumentExpression)
        {
            if (!(argumentExpression is IValueExpression))
            {
                return null;
            }

            var valueExpression = (IValueExpression) argumentExpression;
            if (valueExpression.GetClass() == _clazz)
            {
                return new ArgumentMatchResult
                    {
                        Expression = argumentExpression,
                        Score = 0
                    };
            }

            var intention = Intention.ImplicitCast(
                new TypeExpression(_clazz),
                valueExpression);

            var expressionResult = typeSystem.MakeExpressionFromIntention(intention);
            if (!expressionResult.Ok)
            {
                return null;
            }

            return new ArgumentMatchResult
                {
                    Expression = expressionResult.Expression,
                    Score = 1
                };
        }
    }
}