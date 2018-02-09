namespace TypeSystemExperiment.Types
{
    public class ExpressionResult
    {
        public bool Ok { get; private set; }
        public IExpression Expression { get; private set; }
        public ExpressionResultCode Code { get; private set; }

        public static ExpressionResult Success(IExpression expression)
        {
            return new ExpressionResult
                {
                    Ok = true,
                    Expression = expression
                };
        }

        public static ExpressionResult Fail(ExpressionResultCode code)
        {
            return new ExpressionResult
                {
                    Ok = false,
                    Code = code
                };
        }
    }
}