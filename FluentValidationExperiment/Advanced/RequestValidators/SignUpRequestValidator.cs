using FluentValidation;
using FluentValidationExperiment.Advanced.Requests;

namespace FluentValidationExperiment.Advanced.RequestValidators
{
    public class SignUpRequestValidator : AbstractValidator<SignUpRequest>
    {
        public SignUpRequestValidator()
        {
            RuleFor(signUpRequest => signUpRequest.Email).GoodEmail();
            RuleFor(signUpRequest => signUpRequest.Password1).GoodPassword();
            RuleFor(signUpRequest => signUpRequest.Password2).GoodPassword().Equal(signUpRequest => signUpRequest.Password1);
        }
    }
}