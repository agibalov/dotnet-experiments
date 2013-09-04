using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlServerCe;
using System.IO;
using System.Linq;
using EntityFrameworkInheritanceExperiment.DAL;
using EntityFrameworkInheritanceExperiment.DTO;
using EntityFrameworkInheritanceExperiment.Service.Configuration;
using EntityFrameworkInheritanceExperiment.Service.Domain;
using Ninject;

namespace EntityFrameworkInheritanceExperiment.Service
{
    [Service]
    public class AuthenticationService
    {
        [Inject]
        [Named("ConnectionStringName")]
        public string ConnectionStringName { private get; set; }

        [Inject] public UserService UserService { private get; set; }

        public void Reset()
        {
            var connectionStringBuilder = new SqlCeConnectionStringBuilder(
                ConfigurationManager.ConnectionStrings[ConnectionStringName].ConnectionString);
            var databaseFileName = connectionStringBuilder.DataSource;
            if (File.Exists(databaseFileName))
            {
                File.Delete(databaseFileName);
            }

            using (var context = new UserContext(ConnectionStringName))
            {
                context.Database.Create();
            }
        }

        public UserDTO SignUpWithEmailAndPassword(string email, string password)
        {
            return Run(context => UserService
                .SignUpWithEmailAndPassword(
                    context, 
                    email, 
                    password).AsUserDTO());
        }

        public UserDTO SignInWithEmailAndPassword(string email, string password)
        {
            return Run(context => UserService
                .SignInWithEmailAndPassword(
                    context, 
                    email,
                    password).AsUserDTO());
        }

        public UserDTO AuthenticateWithGoogle(string googleUserId, string email)
        {
            return Run(context => UserService
                .AuthenticateWithGoogle(
                    context,
                    googleUserId,
                    email).AsUserDTO());
        }

        public UserDTO AuthenticateWithFacebook(string facebookUserId, string email)
        {
            return Run(context => UserService
                .AuthenticateWithFacebook(
                    context,
                    facebookUserId,
                    email).AsUserDTO());
        }

        public UserDTO AuthenticateWithTwitter(string twitterUserId, string twitterDisplayName)
        {
            return Run(context => UserService
                .AuthenticateWithTwitter(
                    context, 
                    twitterUserId,
                    twitterDisplayName).AsUserDTO());
        }

        public UserDTO AddEmailAndPassword(int userId, string email, string password)
        {
            return Run(context => UserService
                .AddEmail(
                    context, 
                    userId,
                    email).AsUserDTO());
        }

        public UserDTO AddGoogle(int userId, string googleUserId, string email)
        {
            return Run(context => UserService
                .AddGoogleUserId(
                    context, 
                    userId, 
                    googleUserId,
                    email).AsUserDTO());
        }

        public UserDTO AddFacebook(int userId, string facebookUserId, string email)
        {
            return Run(context => UserService
                .AddFacebookUserId(
                    context, 
                    userId, 
                    facebookUserId,
                    email).AsUserDTO());
        }

        public UserDTO AddTwitter(int userId, string twitterUserId, string twitterDisplayName)
        {
            return Run(context => UserService
                .AddTwitterDisplayName(
                    context, 
                    userId, 
                    twitterUserId,
                    twitterDisplayName).AsUserDTO());
        }

        public UserDTO DeleteAuthenticationMethod(int userId, int authenticationMethodId)
        {
            return Run(context => UserService
                .DeleteAuthenticationMethod(
                    context, 
                    userId,
                    authenticationMethodId).AsUserDTO());
        }

        public IList<UserDTO> GetAllUsers()
        {
            return Run(context => UserService.GetAllUsers(context).Select(u => u.AsUserDTO()).ToList());
        }

        public int GetUserCount()
        {
            return Run(context => UserService.GetUserCount(context));
        }

        public UserDTO GetUser(int userId)
        {
            return Run(context => UserService
                .GetUser(
                    context,
                    userId).AsUserDTO());
        }

        private T Run<T>(Func<UserContext, T> func)
        {
            using (var context = new UserContext(ConnectionStringName))
            {
                return func(context);
            }
        }
    }
}
