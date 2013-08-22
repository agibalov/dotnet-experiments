using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlServerCe;
using System.IO;
using System.Linq;
using System.Text;
using EntityFrameworkInheritanceExperiment.DAL;
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
            throw new NotImplementedException();
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
            throw new NotImplementedException();
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
}
