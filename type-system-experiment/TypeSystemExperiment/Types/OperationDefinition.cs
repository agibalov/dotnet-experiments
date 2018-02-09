using System.Collections.Generic;

namespace TypeSystemExperiment.Types
{
    public class OperationDefinition
    {
        private readonly IntentionCode _intentionCode;
        private readonly Class _resultType;
        private readonly TypedOperationCode _typedOperationCode;
        private readonly IList<IParameterDefinition> _parameterDefinitions;

        public OperationDefinition(
            IntentionCode intentionCode, 
            TypedOperationCode typedOperationCode,
            Class resultType,
            IList<IParameterDefinition> parameterDefinitions)
        {
            _intentionCode = intentionCode;
            _typedOperationCode = typedOperationCode;
            _resultType = resultType;
            _parameterDefinitions = parameterDefinitions;
        }

        public IntentionCode GetOperationCode()
        {
            return _intentionCode;
        }

        public TypedOperationCode GetTypedOperationCode()
        {
            return _typedOperationCode;
        }

        public IList<IParameterDefinition> GetParameterDefinitions()
        {
            return _parameterDefinitions;
        }

        public IExpression MakeExpression(IList<IExpression> expressions)
        {
            return new OperationValueExpression(_typedOperationCode, _resultType, expressions);
        }

        public override string ToString()
        {
            return string.Format("{{{0},{1}}}", _intentionCode, _typedOperationCode);
        }
    }
}