using System;
using System.Collections.Generic;

namespace AOPValidationExperiment.Validation
{
    public class ValidationException : Exception
    {
        public IList<string> FieldsInError { get; private set; }

        public ValidationException(IList<string> fieldsInError)
        {
            FieldsInError = fieldsInError;
        }
    }
}