using FluentValidation;
using SophisticatedDataValidationExperiment.Service.DTO;

namespace SophisticatedDataValidationExperiment.Validation
{
    public class PersonDTOValidator : AbstractValidator<PersonDTO>
    {
        public PersonDTOValidator()
        {
            RuleFor(person => person.Id).NotEqual(0);
            RuleFor(person => person.Name).NotNull().NotEmpty();
        }
    }
}