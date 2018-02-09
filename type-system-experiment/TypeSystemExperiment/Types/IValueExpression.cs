namespace TypeSystemExperiment.Types
{
    public interface IValueExpression : IExpression
    {
        Class GetClass();
    }
}