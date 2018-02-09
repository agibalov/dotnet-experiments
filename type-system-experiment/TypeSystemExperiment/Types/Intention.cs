using System.Collections.Generic;

namespace TypeSystemExperiment.Types
{
    public class Intention
    {
        public IntentionCode IntentionCode { get; private set; }
        public IList<IExpression> Arguments { get; private set; }

        public static Intention Add(IValueExpression leftExpression, IValueExpression rightExpression)
        {
            return new Intention
                {
                    IntentionCode = IntentionCode.Add,
                    Arguments = new List<IExpression> { leftExpression, rightExpression }
                };
        }

        public static Intention ImplicitCast(TypeExpression targetType, IValueExpression sourceExpression)
        {
            return new Intention
                {
                    IntentionCode = IntentionCode.ImplicitCast,
                    Arguments = new List<IExpression> { targetType, sourceExpression }
                };
        }

        public static Intention Assign(ILvalueExpression lvalueExpression, IValueExpression rvalueExpression)
        {
            return new Intention
                {
                    IntentionCode = IntentionCode.Assign,
                    Arguments = new List<IExpression> { lvalueExpression, rvalueExpression }
                };
        }
    }
}