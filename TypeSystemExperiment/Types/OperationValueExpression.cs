using System;
using System.Collections.Generic;

namespace TypeSystemExperiment.Types
{
    public class OperationValueExpression : IValueExpression
    {
        private readonly TypedOperationCode _typedOperationCode;
        private readonly Class _clazz;
        private readonly IList<IExpression> _expressions;

        public OperationValueExpression(TypedOperationCode typedOperationCode, Class clazz, IList<IExpression> expressions)
        {
            _typedOperationCode = typedOperationCode;
            _clazz = clazz;
            _expressions = expressions;
        }

        public Class GetClass()
        {
            return _clazz;
        }

        public TypedOperationCode GetTypedOperationCode()
        {
            return _typedOperationCode;
        }

        public IExpression GetArgument(int index)
        {
            return _expressions[index];
        }

        public override string ToString()
        {
            var expressionsString = String.Join(",", _expressions);
            return string.Format("{{{0}[{1}]}}", _typedOperationCode, expressionsString);
        }
    }
}