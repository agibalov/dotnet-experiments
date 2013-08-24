using System;
using System.Collections.Generic;
using System.Linq;
using EntityFrameworkInheritanceExperiment.DAL;
using EntityFrameworkInheritanceExperiment.DTO;
using EntityFrameworkInheritanceExperiment.Service.Configuration;
using EntityFrameworkInheritanceExperiment.Service.TransactionScripts;
using Ninject;

namespace EntityFrameworkInheritanceExperiment.Service
{
    [Service]
    public class AuthenticationService
    {
        [Inject]
        [Named("ConnectionStringName")]
        public string ConnectionStringName { private get; set; }

        [Inject] public ResetTransactionScript ResetTransactionScript { private get; set; }
        [Inject] public SignUpWithEmailAndPasswordTransactionScript SignUpWithEmailAndPasswordTransactionScript { private get; set; }
        [Inject] public SignInWithEmailAndPasswordTransactionScript SignInWithEmailAndPasswordTransactionScript { private get; set; }

        public void Reset()
        {
            ResetTransactionScript.Reset();
        }

        public UserDTO SignUpWithEmailAndPassword(string email, string password)
        {
            return Run(context => SignUpWithEmailAndPasswordTransactionScript
                .SignUpWithEmailAndPassword(
                    context, 
                    email, 
                    password));
        }

        public UserDTO SignInWithEmailAndPassword(string email, string password)
        {
            return Run(context => SignInWithEmailAndPasswordTransactionScript
                .SignInWithEmailAndPassword(
                    context, 
                    email, 
                    password));
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
            using (var context = new UsersContext(ConnectionStringName))
            {
                return func(context);
            }
        }
    }
}
