using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlServerCe;
using System.IO;
using System.Linq;
using EntityFrameworkInheritanceExperiment.DAL;
using EntityFrameworkInheritanceExperiment.DAL.Entities;
using EntityFrameworkInheritanceExperiment.DTO;

namespace EntityFrameworkInheritanceExperiment
{
    public class AuthenticationService
    {
        private readonly string _connectionStringName;

        public AuthenticationService(string connectionStringName)
        {
            _connectionStringName = connectionStringName;
        }

        public void Reset()
        {
            var connectionStringBuilder = new SqlCeConnectionStringBuilder(
                ConfigurationManager.ConnectionStrings[_connectionStringName].ConnectionString);
            var databaseFileName = connectionStringBuilder.DataSource;
            if (File.Exists(databaseFileName))
            {
                File.Delete(databaseFileName);
            }

            using (var context = new UsersContext(_connectionStringName))
            {
                context.Database.Create();
            }
        }

        public UserDTO SignUpWithEmailAndPassword(string email, string password)
        {
            return Run(context =>
                {
                    var isEmailAreadyUsed = context.AuthenticationMethods.OfType<EmailAuthenticationMethod>().Any(
                        emailAuthenticationMethod => emailAuthenticationMethod.Email == email);
                    if (isEmailAreadyUsed)
                    {
                        throw new EmailAlreadyUsedException();
                    }

                    var user = new User();
                    context.Users.Add(user);

                    var authenticationMethod = new EmailAuthenticationMethod
                        {
                            Email = email,
                            Password = password,
                            User = user
                        };
                    context.AuthenticationMethods.Add(authenticationMethod);
                    
                    context.SaveChanges();
                    
                    return MapUserToUserDTO(authenticationMethod.User);
                });
        }

        private static UserDTO MapUserToUserDTO(User user)
        {
            return new UserDTO
                {
                    UserId = user.UserId,
                    AuthenticationMethods = user
                        .AuthenticationMethods.Select(MapAuthenticationMethodToAuthenticationMethodDTO)
                        .ToList()
                };
        }

        private static AuthenticationMethodDTO MapAuthenticationMethodToAuthenticationMethodDTO(AuthenticationMethod authenticationMethod)
        {
            if (authenticationMethod is EmailAuthenticationMethod)
            {
                return MapEmailPasswordAuthenticationMethodToEmailPasswordAuthenticationMethodDTO((EmailAuthenticationMethod) authenticationMethod);
            }

            if (authenticationMethod is GoogleAuthenticationMethod)
            {
                throw new NotImplementedException();
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

        public UserDTO SignInWithEmailAndPassword(string email, string password)
        {
            throw new NotImplementedException();
        }

        public UserDTO AuthenticateWithGoogleUserId(string googleUserId)
        {
            throw new NotImplementedException();
        }

        public UserDTO AuthenticateWithFacebookUserId(string facebookUserId)
        {
            throw new NotImplementedException();
        }

        public UserDTO AuthenticateWithTwitterDisplayName(string twitterDisplayName)
        {
            throw new NotImplementedException();
        }

        public UserDTO AddEmailAndPassword(int userId, string email, string password)
        {
            throw new NotImplementedException();
        }

        public UserDTO AddGoogleUserId(int userId, string googleUserId)
        {
            throw new NotImplementedException();
        }

        public UserDTO AddFacebookUserId(int userId, string facebookUserId)
        {
            throw new NotImplementedException();
        }

        public UserDTO AddTwitterDisplayName(int userId, string twitterDisplayName)
        {
            throw new NotImplementedException();
        }

        public UserDTO DeleteAuthenticationMethod(int userId, int authenticationMethodId)
        {
            throw new NotImplementedException();
        }

        public IList<UserDTO> GetAllUsers()
        {
            throw new NotImplementedException();
        }

        public int GetUserCount()
        {
            return Run(context =>
                {
                    var userCount = context.Users.Count();
                    return userCount;
                });
        }

        public UserDTO GetUser(int userId)
        {
            throw new NotImplementedException();
        }

        private T Run<T>(Func<UsersContext, T> func)
        {
            using (var context = new UsersContext(_connectionStringName))
            {
                return func(context);
            }
        }
    }

    public abstract class AuthenticationServiceException : Exception
    {
    }

    public class EmailAlreadyUsedException : AuthenticationServiceException
    {
    }
}
