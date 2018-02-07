using System;

namespace AOPValidationExperiment.Validation
{
    public abstract class ValidationAttribute : Attribute
    {
        public abstract bool Test(object x);
    }
}