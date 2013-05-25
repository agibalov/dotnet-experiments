using NUnit.Framework;
using TypeSystemExperiment.Types;

namespace TypeSystemExperiment.Tests
{
    public static class ExpressionExtensions
    {
        public static void NotNull(this IExpression expression)
        {
            Assert.NotNull(expression);
        }

        public static IValueExpression IsValueExpression(this IExpression expression)
        {
            Assert.IsTrue(expression is IValueExpression);
            return (IValueExpression) expression;
        }

        public static TypeExpression IsTypeExpression(this IExpression expression)
        {
            Assert.IsTrue(expression is TypeExpression);
            return (TypeExpression)expression;
        }

        public static ILvalueExpression IsLvalueExpression(this IExpression expression)
        {
            Assert.IsTrue(expression is ILvalueExpression);
            return (ILvalueExpression)expression;
        }

        public static OperationValueExpression IsOperationExpression(this IValueExpression expression)
        {
            Assert.IsTrue(expression is OperationValueExpression);
            return (OperationValueExpression)expression;
        }

        public static LiteralValueExpression IsLiteralExpression(this IValueExpression expression)
        {
            Assert.IsTrue(expression is LiteralValueExpression);
            return (LiteralValueExpression)expression;
        }

        public static T WithType<T>(this T expression, Class clazz) where T : IValueExpression
        {
            Assert.AreEqual(clazz, expression.GetClass());
            return expression;
        }

        public static TypeExpression WithType(this TypeExpression expression, Class clazz)
        {
            Assert.AreEqual(clazz, expression.GetClass());
            return expression;
        }

        public static OperationValueExpression WithOperationCode(this OperationValueExpression expression, TypedOperationCode typedOperationCode)
        {
            Assert.AreEqual(typedOperationCode, expression.GetTypedOperationCode());
            return expression;
        }
    }
}
