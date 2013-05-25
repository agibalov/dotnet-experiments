using NUnit.Framework;
using TypeSystemExperiment.Types;

namespace TypeSystemExperiment.Tests
{
    [TestFixture]
    public class Tests
    {
        [Test]
        public void AddIntAndInt()
        {
            var intType = new Class("int");
            var doubleType = new Class("double");
            var typeSystem = TypeSystem.MakeDefaultTypeSystem(intType, doubleType);

            var intention = Intention.Add(
                new LiteralValueExpression(intType),
                new LiteralValueExpression(intType));

            var result = typeSystem.MakeExpressionFromIntention(intention);

            var operationExpression = result.Expression
                .IsValueExpression()
                .IsOperationExpression()
                .WithOperationCode(TypedOperationCode.AddInt)
                .WithType(intType);

            operationExpression.GetArgument(0)
                .IsValueExpression()
                .IsLiteralExpression()
                .WithType(intType);

            operationExpression.GetArgument(1)
                .IsValueExpression()
                .IsLiteralExpression()
                .WithType(intType);
        }

        [Test]
        public void AddDoubleAndDouble()
        {
            var intType = new Class("int");
            var doubleType = new Class("double");
            var typeSystem = TypeSystem.MakeDefaultTypeSystem(intType, doubleType);

            var intention = Intention.Add(
                new LiteralValueExpression(doubleType),
                new LiteralValueExpression(doubleType));

            var result = typeSystem.MakeExpressionFromIntention(intention);

            var operationExpression = result.Expression
                .IsValueExpression()
                .IsOperationExpression()
                .WithOperationCode(TypedOperationCode.AddDouble)
                .WithType(doubleType);

            operationExpression.GetArgument(0)
                .IsValueExpression()
                .IsLiteralExpression()
                .WithType(doubleType);

            operationExpression.GetArgument(1)
                .IsValueExpression()
                .IsLiteralExpression()
                .WithType(doubleType);
        }

        [Test]
        public void AddDoubleAndInt()
        {
            var intType = new Class("int");
            var doubleType = new Class("double");
            var typeSystem = TypeSystem.MakeDefaultTypeSystem(intType, doubleType);

            var intention = Intention.Add(
                new LiteralValueExpression(doubleType),
                new LiteralValueExpression(intType));

            var result = typeSystem.MakeExpressionFromIntention(intention);

            var operationExpression = result.Expression
                .IsValueExpression()
                .IsOperationExpression()
                .WithOperationCode(TypedOperationCode.AddDouble)
                .WithType(doubleType);

            operationExpression.GetArgument(0)
                .IsValueExpression()
                .IsLiteralExpression()
                .WithType(doubleType);

            var op = operationExpression.GetArgument(1)
                .IsValueExpression()
                .IsOperationExpression()
                .WithOperationCode(TypedOperationCode.CastIntToDouble);
            op.GetArgument(0)
                .IsTypeExpression()
                .WithType(doubleType);
            op.GetArgument(1)
                .IsValueExpression()
                .IsLiteralExpression()
                .WithType(intType);
        }

        [Test]
        public void AddIntAndDouble()
        {
            var intType = new Class("int");
            var doubleType = new Class("double");
            var typeSystem = TypeSystem.MakeDefaultTypeSystem(intType, doubleType);

            var intention = Intention.Add(
                new LiteralValueExpression(intType),
                new LiteralValueExpression(doubleType));

            var result = typeSystem.MakeExpressionFromIntention(intention);

            var operationExpression = result.Expression
                .IsValueExpression()
                .IsOperationExpression()
                .WithOperationCode(TypedOperationCode.AddDouble)
                .WithType(doubleType);

            var op = operationExpression.GetArgument(0)
                .IsValueExpression()
                .IsOperationExpression()
                .WithOperationCode(TypedOperationCode.CastIntToDouble);
            op.GetArgument(0)
                .IsTypeExpression()
                .WithType(doubleType);
            op.GetArgument(1)
                .IsValueExpression()
                .IsLiteralExpression()
                .WithType(intType);

            operationExpression.GetArgument(1)
                .IsValueExpression()
                .IsLiteralExpression()
                .WithType(doubleType);
        }

        [Test]
        public void AssignIntToIntVariable()
        {
            var intType = new Class("int");
            var doubleType = new Class("double");
            var typeSystem = TypeSystem.MakeDefaultTypeSystem(intType, doubleType);

            var intention = Intention.Assign(
                new VariableLvalueExpression(intType), 
                new LiteralValueExpression(intType));

            var result = typeSystem.MakeExpressionFromIntention(intention).Expression
                .IsValueExpression()
                .IsOperationExpression()
                .WithOperationCode(TypedOperationCode.AssignIntToIntVariable);

            result.GetArgument(0)
                .IsLvalueExpression()
                .WithType(intType);

            result.GetArgument(1)
                .IsValueExpression()
                .IsLiteralExpression()
                .WithType(intType);
        }

        [Test]
        public void AssignDoubleToDoubleVariable()
        {
            var intType = new Class("int");
            var doubleType = new Class("double");
            var typeSystem = TypeSystem.MakeDefaultTypeSystem(intType, doubleType);

            var intention = Intention.Assign(
                new VariableLvalueExpression(doubleType),
                new LiteralValueExpression(doubleType));

            var result = typeSystem.MakeExpressionFromIntention(intention).Expression
                .IsValueExpression()
                .IsOperationExpression()
                .WithOperationCode(TypedOperationCode.AssignDoubleToDoubleVariable);

            result.GetArgument(0)
                .IsLvalueExpression()
                .WithType(doubleType);

            result.GetArgument(1)
                .IsValueExpression()
                .IsLiteralExpression()
                .WithType(doubleType);
        }

        [Test]
        public void AssignIntToDoubleVariable()
        {
            var intType = new Class("int");
            var doubleType = new Class("double");
            var typeSystem = TypeSystem.MakeDefaultTypeSystem(intType, doubleType);

            var intention = Intention.Assign(
                new VariableLvalueExpression(doubleType),
                new LiteralValueExpression(intType));

            var result = typeSystem.MakeExpressionFromIntention(intention).Expression
                .IsValueExpression()
                .IsOperationExpression()
                .WithOperationCode(TypedOperationCode.AssignDoubleToDoubleVariable);

            result.GetArgument(0)
                .IsLvalueExpression()
                .WithType(doubleType);

            var op = result.GetArgument(1)
                .IsValueExpression()
                .IsOperationExpression()
                .WithOperationCode(TypedOperationCode.CastIntToDouble);

            op.GetArgument(0)
                .IsTypeExpression()
                .WithType(doubleType);

            op.GetArgument(1)
                .IsValueExpression()
                .WithType(intType);
        }
    }
}