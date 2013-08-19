using System;

namespace dotnet_mongodb_experiment.DTO
{
    public class AuthenticationDTO
    {
        public String SessionToken { get; set; }
        public String UserName { get; set; }
    }
}