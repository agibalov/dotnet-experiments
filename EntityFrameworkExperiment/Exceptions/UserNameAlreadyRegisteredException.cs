using EntityFrameworkExperiment.DTO;

namespace EntityFrameworkExperiment.Exceptions
{
    public class UserNameAlreadyRegisteredException : BlogServiceException
    {
        public UserNameAlreadyRegisteredException()
            : base(ServiceError.UserNameAlreadyRegistered)
        {}
    }
}