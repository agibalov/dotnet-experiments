namespace TypeSystemExperiment.Types
{
    public class LiteralValueExpression : IValueExpression
    {
        private readonly Class _clazz;

        public LiteralValueExpression(Class clazz)
        {
            _clazz = clazz;
        }

        public Class GetClass()
        {
            return _clazz;
        }

        public override string ToString()
        {
            return string.Format("Literal({0})", _clazz);
        }
    }
}