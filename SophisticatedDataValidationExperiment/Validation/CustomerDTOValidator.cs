using FluentValidation;
using SophisticatedDataValidationExperiment.Service.DTO;

namespace SophisticatedDataValidationExperiment.Validation
{
    public class CustomerDTOValidator : AbstractValidator<CustomerDTO>
    {
        public CustomerDTOValidator(PersonDTOValidator personDtoValidator)
        {
            RuleFor(customer => customer.Id).NotEqual(0);
            RuleFor(customer => customer.Person).SetValidator(personDtoValidator);
        }
    }
}