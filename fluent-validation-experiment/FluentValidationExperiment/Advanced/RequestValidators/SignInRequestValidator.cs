using FluentValidation;
using FluentValidationExperiment.Advanced.Requests;

namespace FluentValidationExperiment.Advanced.RequestValidators
{
    public class SignInRequestValidator : AbstractValidator<SignInRequest>
    {
        public SignInRequestValidator()
        {
            RuleFor(signInRequest => signInRequest.Email).GoodEmail();
            RuleFor(signInRequest => signInRequest.Password).GoodPassword();
        }
    }
}