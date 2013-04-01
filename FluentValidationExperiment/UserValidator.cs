using FluentValidation;

namespace FluentValidationExperiment
{
    public class UserValidator : AbstractValidator<User>
    {
        public UserValidator()
        {
            RuleFor(user => user.UserName).NotEmpty();
            RuleFor(user => user.Email).EmailAddress().NotEmpty();
            RuleFor(user => user.Password).Length(6, 32);
        }
    }
}