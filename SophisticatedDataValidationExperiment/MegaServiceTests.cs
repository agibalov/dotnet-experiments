using System.Text;
using FluentValidation;
using NUnit.Framework;
using Newtonsoft.Json;
using Ninject;
using SophisticatedDataValidationExperiment.Service;
using SophisticatedDataValidationExperiment.Service.DTO;
using SophisticatedDataValidationExperiment.Validation;
using Ninject.Extensions.Conventions;

namespace SophisticatedDataValidationExperiment
{
    public class MegaServiceTests
    {
        private readonly OrderDTOValidator _orderDtoValidator;

        public MegaServiceTests()
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

        [Test]
        public void Test()
        {
            var service = new MegaService();
            var order = service.GetOrder();
            AssertAdequateOrder(order);
        }

        private void AssertAdequateOrder(OrderDTO order)
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