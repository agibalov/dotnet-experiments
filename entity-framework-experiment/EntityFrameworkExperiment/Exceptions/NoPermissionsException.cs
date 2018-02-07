using EntityFrameworkExperiment.DTO;

namespace EntityFrameworkExperiment.Exceptions
{
    public class NoPermissionsException : BlogServiceException
    {
        public NoPermissionsException()
            : base(ServiceError.NoPermissions)
        { }
    }
}