using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlServerCe;
using System.IO;
using System.Linq;
using EntityFrameworkInheritanceExperiment.DAL;
using EntityFrameworkInheritanceExperiment.DTO;
using EntityFrameworkInheritanceExperiment.Service.Domain;
using Ninject;

namespace EntityFrameworkInheritanceExperiment.Service
{
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
            return UserService.SignUpWithEmailAndPassword(
                email, 
                password).AsUserDTO();
        }

        public UserDTO SignInWithEmailAndPassword(string email, string password)
        {
            return UserService.SignInWithEmailAndPassword(
                email,
                password).AsUserDTO();
        }

        public UserDTO AuthenticateWithGoogle(string googleUserId, string email)
        {
            return UserService.AuthenticateWithGoogle(
                googleUserId,
                email).AsUserDTO();
        }

        public UserDTO AuthenticateWithFacebook(string facebookUserId, string email)
        {
            return UserService.AuthenticateWithFacebook(
                facebookUserId,
                email).AsUserDTO();
        }

        public UserDTO AuthenticateWithTwitter(string twitterUserId, string twitterDisplayName)
        {
            return UserService.AuthenticateWithTwitter(
                twitterUserId,
                twitterDisplayName).AsUserDTO();
        }

        public UserDTO AddEmailAndPassword(int userId, string email, string password)
        {
            return UserService.AddEmail(
                userId,
                email).AsUserDTO();
        }

        public UserDTO AddGoogle(int userId, string googleUserId, string email)
        {
            return UserService.AddGoogle(
                userId, 
                googleUserId,
                email).AsUserDTO();
        }

        public UserDTO AddFacebook(int userId, string facebookUserId, string email)
        {
            return UserService.AddFacebook(
                userId, 
                facebookUserId,
                email).AsUserDTO();
        }

        public UserDTO AddTwitter(int userId, string twitterUserId, string twitterDisplayName)
        {
            return UserService.AddTwitter(
                userId, 
                twitterUserId,
                twitterDisplayName).AsUserDTO();
        }

        public UserDTO DeleteAuthenticationMethod(int userId, int authenticationMethodId)
        {
            return UserService.DeleteAuthenticationMethod(
                userId,
                authenticationMethodId).AsUserDTO();
        }

        public IList<UserDTO> GetAllUsers()
        {
            return UserService.GetAllUsers().Select(u => u.AsUserDTO()).ToList();
        }

        public int GetUserCount()
        {
            return UserService.GetUserCount();
        }

        public UserDTO GetUser(int userId)
        {
            return UserService.GetUser(userId).AsUserDTO();
        }
    }
}
