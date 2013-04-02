using EntityFrameworkExperiment.DTO;

namespace EntityFrameworkExperiment.Exceptions
{
    public class NoSuchCommentException : BlogServiceException
    {
        public NoSuchCommentException()
            : base(ServiceError.NoSuchComment)
        { }
    }
}