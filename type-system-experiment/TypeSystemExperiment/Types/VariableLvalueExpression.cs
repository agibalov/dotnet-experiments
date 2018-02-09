namespace TypeSystemExperiment.Types
{
    public class VariableLvalueExpression : ILvalueExpression
    {
        private readonly Class _clazz;

        public VariableLvalueExpression(Class clazz)
        {
            _clazz = clazz;
        }

        public Class GetClass()
        {
            return _clazz;
        }

        public override string ToString()
        {
            return string.Format("Variable({0})", _clazz);
        }
    }
}