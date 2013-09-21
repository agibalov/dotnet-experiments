using System.Text;
using FluentValidation;
using NUnit.Framework;
using Newtonsoft.Json;
using Ninject;
using Ninject.Extensions.Conventions;
using SophisticatedDataValidationExperiment.Service.DTO;
using SophisticatedDataValidationExperiment.Validation;

namespace SophisticatedDataValidationExperiment
{
    public abstract class AbstractTests
    {
        private readonly OrderDTOValidator _orderDtoValidator;

        protected AbstractTests()
        {
            var kernel = new StandardKernel();
            kernel.Bind(x => x
                .FromThisAssembly()
                .SelectAllClasses()
                .InheritedFrom(typeof (AbstractValidator<>))
                .BindToSelf()
                .Configure(c => c.InSingletonScope()));

            _orderDtoValidator = kernel.Get<OrderDTOValidator>();
        }

        protected void AssertAdequateOrder(OrderDTO order)
        {
            var validationResult = _orderDtoValidator.Validate(order);
            if (validationResult.IsValid)
            {
                return;
            }

            var stringBuilder = new StringBuilder();

            stringBuilder.Append(JsonConvert.SerializeObject(order, Formatting.Indented));
            stringBuilder.AppendLine();
            stringBuilder.AppendLine();
            stringBuilder.AppendLine("is bad because:");
            stringBuilder.AppendLine();

            foreach (var error in validationResult.Errors)
            {
                stringBuilder.AppendFormat(
                    "{0} {1} (actual: {2})\n",
                    error.PropertyName,
                    error.ErrorMessage,
                    error.AttemptedValue);
            }

            Assert.Fail(stringBuilder.ToString());
        }
    }
}