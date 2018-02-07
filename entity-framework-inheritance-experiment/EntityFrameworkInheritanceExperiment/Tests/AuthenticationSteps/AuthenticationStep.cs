using System;
using EntityFrameworkInheritanceExperiment.DTO;
using EntityFrameworkInheritanceExperiment.Service;

namespace EntityFrameworkInheritanceExperiment.Tests.AuthenticationSteps
{
    public abstract class AuthenticationStep : IComparable<AuthenticationStep>
    {
        public int CompareTo(AuthenticationStep other)
        {
            return string.CompareOrdinal(ToString(), other.ToString());
        }
        
        public abstract UserDTO Run(AuthenticationService authenticationService);
        public abstract override string ToString();
    }
}