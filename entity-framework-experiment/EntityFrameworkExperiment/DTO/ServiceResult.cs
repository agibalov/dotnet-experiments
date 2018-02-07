namespace EntityFrameworkExperiment.DTO
{
    public class ServiceResult<T>
    {
        public bool Ok { get; set; }
        public T Payload { get; set; }
        public ServiceError ServiceError { get; set; }

        public static ServiceResult<T> Success(T payload)
        {
            return new ServiceResult<T>
                {
                    Ok = true, 
                    Payload = payload
                };
        }

        public static ServiceResult<T> Failure(ServiceError serviceError)
        {
            return new ServiceResult<T>
                {
                    Ok = false,
                    ServiceError = serviceError
                };
        }
    }
}