using System;
using EntityFrameworkExperiment.DTO;

namespace EntityFrameworkExperiment.Exceptions
{
    public abstract class BlogServiceException : Exception
    {
        public ServiceError Error { get; private set; }

        protected BlogServiceException(ServiceError serviceError)
        {
            Error = serviceError;
        }
    }
}