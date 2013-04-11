using System;

namespace AOPValidationExperiment.Validation
{
    public abstract class ValidationAttribute : Attribute
    {
        public abstract bool IsOk(object x);
    }
}