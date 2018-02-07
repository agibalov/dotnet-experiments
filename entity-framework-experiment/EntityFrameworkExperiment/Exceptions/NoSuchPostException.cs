using EntityFrameworkExperiment.DTO;

namespace EntityFrameworkExperiment.Exceptions
{
    public class NoSuchPostException : BlogServiceException
    {
        public NoSuchPostException()
            : base(ServiceError.NoSuchPost)
        { }
    }
}