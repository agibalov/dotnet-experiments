using System;
using AOPValidationExperiment.Validation;

namespace AOPValidationExperiment
{
    [ValidateParameters]
    public class TestApi
    {
        public virtual void SignIn(
            [UserName] string userName, 
            [Password] string password)
        {
            Console.WriteLine("SignIn: {0}, {1}", userName, password);
        }
    }
}