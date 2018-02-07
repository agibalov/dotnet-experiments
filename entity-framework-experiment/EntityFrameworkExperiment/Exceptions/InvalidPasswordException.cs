using EntityFrameworkExperiment.DTO;

namespace EntityFrameworkExperiment.Exceptions
{
    public class InvalidPasswordException : BlogServiceException
    {
        public InvalidPasswordException()
            : base(ServiceError.InvalidPassword)
        { }
    }
}