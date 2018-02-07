using System.Collections.Generic;
using System.Linq;
using Ninject.Extensions.Interception;

namespace AOPValidationExperiment.Validation
{
    public class ValidatingInterceptor : IInterceptor
    {
        public void Intercept(IInvocation invocation)
        {
            var fieldsInError = new List<string>();

            var targetMethod = invocation.Request.Method;
            var parameters = targetMethod.GetParameters();
            for (var i = 0; i < parameters.Length; ++i)
            {
                var parameter = parameters[i];
                var parameterValidationAttributes = parameter
                    .GetCustomAttributes(typeof (ValidationAttribute), true)
                    .Cast<ValidationAttribute>()
                    .ToList();
                if (parameterValidationAttributes.Count == 0)
                {
                    continue;
                }

                var argument = invocation.Request.Arguments[i];
                var isBadArgument = parameterValidationAttributes
                    .Any(validator => !validator.Test(argument));
                if (isBadArgument)
                {
                    fieldsInError.Add(parameter.Name);
                }
            }

            if (fieldsInError.Count > 0)
            {
                throw new ValidationException(fieldsInError);
            }

            invocation.Proceed();
        }
    }
}