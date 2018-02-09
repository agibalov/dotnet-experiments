using FluentValidation;
using SophisticatedDataValidationExperiment.Service.DTO;

namespace SophisticatedDataValidationExperiment.Validation
{
    public class OrderDTOValidator : AbstractValidator<OrderDTO>
    {
        public OrderDTOValidator(CustomerDTOValidator customerDtoValidator, ItemDTOValidator itemDtoValidator)
        {
            RuleFor(order => order.Id).NotEqual(0);
            RuleFor(order => order.Customer).SetValidator(customerDtoValidator);
            RuleForEach(order => order.Items).SetValidator(itemDtoValidator);
        }
    }
}