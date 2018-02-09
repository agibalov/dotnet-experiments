using System;
using SophisticatedDataValidationExperiment.Service;
using SophisticatedDataValidationExperiment.Validation;

namespace SophisticatedDataValidationExperiment
{
    class Program
    {
        static void Main(string[] args)
        {
            var x = new MegaService().GetOrder();

            var personDtoValidator = new PersonDTOValidator();
            var customerDtoValidator = new CustomerDTOValidator(personDtoValidator);
            var itemDtoValidator = new ItemDTOValidator();
            var orderDtoValidator = new OrderDTOValidator(customerDtoValidator, itemDtoValidator);
            var validationResults = orderDtoValidator.Validate(x);
            if (validationResults.IsValid)
            {
                Console.WriteLine("OK!");
            }
            else
            {
                Console.WriteLine("Errors");
                foreach (var error in validationResults.Errors)
                {
                    // build parser with https://github.com/Dervall/Piglet to read this?
                    // alternative: https://github.com/sucaba/IronTextLibrary
                    Console.WriteLine(
                        "{0} {1} (actual: {2})", 
                        error.PropertyName, 
                        error.ErrorMessage, 
                        error.AttemptedValue);
                }
            }
        }
    }
}
