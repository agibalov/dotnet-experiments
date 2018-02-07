using FluentValidation;

namespace FluentValidationExperiment.Advanced
{
    public static class ValidationConfiguration
    {
        public static IRuleBuilder<T, string> GoodPassword<T>(this IRuleBuilder<T, string> ruleBuilder)
        {
            return ruleBuilder
                .NotEmpty()
                .Length(6, 32);
        }

        public static IRuleBuilder<T, string> GoodEmail<T>(this IRuleBuilder<T, string> ruleBuilder)
        {
            return ruleBuilder
                .NotEmpty()
                .EmailAddress();
        }
    }
}