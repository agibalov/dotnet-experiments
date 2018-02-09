namespace TypeSystemExperiment.Types
{
    public interface IParameterDefinition
    {
        ArgumentMatchResult MatchArgument(TypeSystem typeSystem, IExpression argumentExpression);
    }
}