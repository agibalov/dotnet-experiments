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
            if (authenticationMethod is PasswordAuthenticationMethod)
            {
                return MapEmailPasswordAuthenticationMethodToEmailPasswordAuthenticationMethodDTO((PasswordAuthenticationMethod) authenticationMethod);
            }

            if (authenticationMethod is GoogleAuthenticationMethod)
            {
                return MapGoogleAuthenticationMethodToGoogleAuthenticationMethodDTO((GoogleAuthenticationMethod) authenticationMethod);
            }

            if (authenticationMethod is FacebookAuthenticationMethod)
            {
                return MapFacebookAuthenticationMethodToFacebookAuthenticationMethodDTO((FacebookAuthenticationMethod) authenticationMethod);
            }

            if (authenticationMethod is TwitterAuthenticationMethod)
            {
                return MapTwitterAuthenticationMethodToTwitterAuthenticationMethodDTO((TwitterAuthenticationMethod) authenticationMethod);
            }
            
            throw new NotImplementedException();
        }

        private static PasswordAuthenticationMethodDTO
            MapEmailPasswordAuthenticationMethodToEmailPasswordAuthenticationMethodDTO(
            PasswordAuthenticationMethod authenticationMethod)
        {
            return new PasswordAuthenticationMethodDTO
                {
                    AuthenticationMethodId = authenticationMethod.AuthenticationMethodId,
                    Password = authenticationMethod.Password
                };
        }

        private static GoogleAuthenticationMethodDTO MapGoogleAuthenticationMethodToGoogleAuthenticationMethodDTO(GoogleAuthenticationMethod authenticationMethod)
        {
            return new GoogleAuthenticationMethodDTO
                {
                    AuthenticationMethodId = authenticationMethod.AuthenticationMethodId,
                    GoogleUserId = authenticationMethod.GoogleUserId,
                };
        }

        private static FacebookAuthenticationMethodDTO MapFacebookAuthenticationMethodToFacebookAuthenticationMethodDTO(FacebookAuthenticationMethod authenticationMethod)
        {
            return new FacebookAuthenticationMethodDTO
                {
                    AuthenticationMethodId = authenticationMethod.AuthenticationMethodId,
                    FacebookUserId = authenticationMethod.FacebookUserId,
                };
        }

        private static TwitterAuthenticationMethodDTO MapTwitterAuthenticationMethodToTwitterAuthenticationMethodDTO(
            TwitterAuthenticationMethod authenticationMethod)
        {
            return new TwitterAuthenticationMethodDTO
                {
                    AuthenticationMethodId = authenticationMethod.AuthenticationMethodId,
                    TwitterUserId = authenticationMethod.TwitterUserId,
                };
        }
    }
}