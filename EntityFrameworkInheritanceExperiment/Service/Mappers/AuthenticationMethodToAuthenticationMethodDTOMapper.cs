using System;
using EntityFrameworkInheritanceExperiment.DAL.Entities;
using EntityFrameworkInheritanceExperiment.DTO;
using EntityFrameworkInheritanceExperiment.Service.Configuration;

namespace EntityFrameworkInheritanceExperiment.Service.Mappers
{
    [Mapper]
    public class AuthenticationMethodToAuthenticationMethodDTOMapper
    {
        public AuthenticationMethodDTO MapAuthenticationMethodToAuthenticationMethodDTO(AuthenticationMethod authenticationMethod)
        {
            if (authenticationMethod is EmailAuthenticationMethod)
            {
                return MapEmailPasswordAuthenticationMethodToEmailPasswordAuthenticationMethodDTO((EmailAuthenticationMethod) authenticationMethod);
            }

            if (authenticationMethod is GoogleAuthenticationMethod)
            {
                return MapGoogleAuthenticationMethodToGoogleAuthenticationMethodDTO((GoogleAuthenticationMethod) authenticationMethod);
            }

            if (authenticationMethod is FacebookAuthenticationMethod)
            {
                throw new NotImplementedException();
            }

            if (authenticationMethod is TwitterAuthenticationMethod)
            {
                throw new NotImplementedException();
            }
            
            throw new NotImplementedException();
        }

        private static EmailAuthenticationMethodDTO
            MapEmailPasswordAuthenticationMethodToEmailPasswordAuthenticationMethodDTO(
            EmailAuthenticationMethod authenticationMethod)
        {
            return new EmailAuthenticationMethodDTO
                {
                    AuthenticationMethodId = authenticationMethod.AuthenticationMethodId,
                    Email = authenticationMethod.Email,
                    Password = authenticationMethod.Password
                };
        }

        private static GoogleAuthenticationMethodDTO MapGoogleAuthenticationMethodToGoogleAuthenticationMethodDTO(GoogleAuthenticationMethod authenticationMethod)
        {
            return new GoogleAuthenticationMethodDTO
                {
                    AuthenticationMethodId = authenticationMethod.AuthenticationMethodId,
                    GoogleUserId = authenticationMethod.GoogleUserId,
                    Email = authenticationMethod.Email
                };
        }
    }
}