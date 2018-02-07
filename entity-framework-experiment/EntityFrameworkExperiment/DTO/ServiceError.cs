namespace EntityFrameworkExperiment.DTO
{
    public enum ServiceError
    {
        UserNameAlreadyRegistered,
        NoSuchUser,
        InvalidPassword,
        InvalidSession,
        NoSuchPost,
        NoSuchComment,
        NoPermissions
    }
}