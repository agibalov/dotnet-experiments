using System;
using System.Collections.Generic;
using System.Linq;

namespace TypeSystemExperiment.Types
{
    public class TypeSystem
    {
        private readonly IList<OperationDefinition> _operationDefinitions = new List<OperationDefinition>();

        public void DefineOperation(
            IntentionCode intentionCode, 
            TypedOperationCode typedOperationCode,
            Class resultType,
            IList<IParameterDefinition> parameterDefinitions)
        {
            var operationDefinition = new OperationDefinition(
                intentionCode, 
                typedOperationCode, 
                resultType, 
                parameterDefinitions);

            _operationDefinitions.Add(operationDefinition);
        }

        public void DefineOperation(
            IList<IntentionCode> operationCodes,
            TypedOperationCode typedOperationCode,
            Class resultType,
            IList<IParameterDefinition> parameterDefinitions)
        {
            foreach (var operationCode in operationCodes)
            {
                DefineOperation(operationCode, typedOperationCode, resultType, parameterDefinitions);
            }
        }

        public ExpressionResult MakeExpressionFromIntention(Intention intention)
        {
            var operationCandidates = _operationDefinitions
                .Where(operationDefinition => operationDefinition.GetOperationCode() == intention.IntentionCode)
                .ToList();
            if (operationCandidates.Count == 0)
            {
                return ExpressionResult.Fail(ExpressionResultCode.NoOperationsForGivenIntention);
            }

            var candidateOperations = new List<CandidateOperation>();
            foreach (var operationCandidate in operationCandidates)
            {
                var parameterDefinitions = operationCandidate.GetParameterDefinitions();
                if (parameterDefinitions.Count != intention.Arguments.Count)
                {
                    continue;
                }

                var matched = true;
                var arguments = new List<IExpression>();
                var totalScore = 0;
                for (var i = 0; i < parameterDefinitions.Count; ++i)
                {
                    var parameterDefinition = parameterDefinitions[i];
                    var argumentExpression = intention.Arguments[i];
                    var argumentMatchResult = parameterDefinition.MatchArgument(this, argumentExpression);
                    if (argumentMatchResult == null)
                    {
                        matched = false;
                        break;
                    }

                    arguments.Add(argumentMatchResult.Expression);
                    totalScore += argumentMatchResult.Score;
                }

                if (!matched)
                {
                    continue;
                }

                candidateOperations.Add(new CandidateOperation
                    {
                        OperationDefinition = operationCandidate,
                        Arguments = arguments,
                        Score = totalScore
                    });
            }

            if (candidateOperations.Count == 0)
            {
                return ExpressionResult.Fail(ExpressionResultCode.NoOperationsMatchingArguments);
            }

            var bestScore = candidateOperations.Min(x => x.Score);
            if (candidateOperations.Count(x => x.Score == bestScore) > 1)
            {
                return ExpressionResult.Fail(ExpressionResultCode.MoreThanOneBestCandidateForGivenIntention);
            }

            var bestCandidate = candidateOperations.Single(x => x.Score == bestScore);
            var bestOperationDefinition = bestCandidate.OperationDefinition;
            var expression = bestOperationDefinition.MakeExpression(bestCandidate.Arguments);
            
            return ExpressionResult.Success(expression);
        }

        public static TypeSystem MakeDefaultTypeSystem(Class intType, Class doubleType)
        {
            var typeSystem = new TypeSystem();
            typeSystem.DefineOperation(
                IntentionCode.Add,
                TypedOperationCode.AddInt,
                intType,
                new List<IParameterDefinition>
                    {
                        ValueExpression(intType), 
                        ValueExpression(intType)
                    });
            typeSystem.DefineOperation(
                IntentionCode.Add,
                TypedOperationCode.AddDouble,
                doubleType,
                new List<IParameterDefinition>
                    {
                        ValueExpression(doubleType), 
                        ValueExpression(doubleType)
                    });
            typeSystem.DefineOperation(
                new List<IntentionCode> { IntentionCode.ImplicitCast, IntentionCode.ExplicitCast },
                TypedOperationCode.CastIntToDouble,
                doubleType,
                new List<IParameterDefinition>
                    {
                        TypeExpression(doubleType), 
                        ValueExpression(intType)
                    });
            typeSystem.DefineOperation(
                new List<IntentionCode> { IntentionCode.ExplicitCast },
                TypedOperationCode.CastDoubleToInt,
                intType,
                new List<IParameterDefinition>
                    {
                        TypeExpression(intType), 
                        ValueExpression(doubleType)
                    });
            typeSystem.DefineOperation(
                IntentionCode.Assign, 
                TypedOperationCode.AssignIntToIntVariable,
                intType,
                new List<IParameterDefinition>
                    {
                        VariableExpression(intType),
                        ValueExpression(intType)
                    });
            typeSystem.DefineOperation(
                IntentionCode.Assign,
                TypedOperationCode.AssignDoubleToDoubleVariable,
                doubleType,
                new List<IParameterDefinition>
                    {
                        VariableExpression(doubleType),
                        ValueExpression(doubleType)
                    });

            return typeSystem;
        }

        private static IParameterDefinition VariableExpression(Class clazz)
        {
            return new VariableLvalueExpressionOfTypeParameterDefinition(clazz);
        }

        private static IParameterDefinition ValueExpression(Class clazz)
        {
            return new ValueExpressionOfTypeParameterDefinition(clazz);
        }

        private static IParameterDefinition TypeExpression(Class clazz)
        {
            return new TypeExpressionOfTypeParameterDefinition(clazz);
        }
    }
}