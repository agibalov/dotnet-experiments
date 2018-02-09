using FluentValidation;
using SophisticatedDataValidationExperiment.Service.DTO;

namespace SophisticatedDataValidationExperiment.Validation
{
    public class ItemDTOValidator : AbstractValidator<ItemDTO>
    {
        public ItemDTOValidator()
        {
            RuleFor(item => item.ItemId).NotEqual(0);
            RuleFor(item => item.ItemName).NotNull().NotEmpty();
        }
    }
}