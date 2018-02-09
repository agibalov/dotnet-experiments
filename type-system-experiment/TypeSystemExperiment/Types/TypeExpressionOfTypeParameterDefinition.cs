namespace TypeSystemExperiment.Types
{
    public class TypeExpressionOfTypeParameterDefinition : IParameterDefinition
    {
        private readonly Class _clazz;

        public TypeExpressionOfTypeParameterDefinition(Class clazz)
        {
            _clazz = clazz;
        }

        public ArgumentMatchResult MatchArgument(TypeSystem typeSystem, IExpression argumentExpression)
        {
            if (!(argumentExpression is TypeExpression))
            {
                return null;
            }

            var typeExpression = (TypeExpression)argumentExpression;
            if (typeExpression.GetClass() == _clazz)
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