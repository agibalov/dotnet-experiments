namespace TypeSystemExperiment.Types
{
    public class TypeExpression : IExpression
    {
        private readonly Class _clazz;

        public TypeExpression(Class clazz)
        {
            _clazz = clazz;
        }

        public Class GetClass()
        {
            return _clazz;
        }
    }
}