using EntityFrameworkExperiment.DTO;

namespace EntityFrameworkExperiment.Exceptions
{
    public class InvalidSessionException : BlogServiceException
    {
        public InvalidSessionException()
            : base(ServiceError.InvalidSession)
        {}
    }
}