using EntityFrameworkExperiment.DTO;

namespace EntityFrameworkExperiment.Exceptions
{
    public class NoSuchUserException : BlogServiceException
    {
        public NoSuchUserException()
            : base(ServiceError.NoSuchUser)
        { }
    }
}