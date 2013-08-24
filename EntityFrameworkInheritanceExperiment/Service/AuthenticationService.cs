using System;
using System.Collections.Generic;
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
        [Inject] public AuthenticateWithGoogleTransactionScript AuthenticateWithGoogleTransactionScript { private get; set; }
        [Inject] public AuthenticateWithFacebookTransactionScript AuthenticateWithFacebookTransactionScript { private get; set; }
        [Inject] public AuthenticateWithTwitterDisplayNameTransactionScript AuthenticateWithTwitterTransactionScript { private get; set; }
        [Inject] public AddEmailAndPasswordTransactionScript AddEmailAndPasswordTransactionScript { private get; set; }
        [Inject] public AddGoogleUserIdTransactionScript AddGoogleUserIdTransactionScript { private get; set; }
        [Inject] public AddFacebookUserIdTransactionScript AddFacebookUserIdTransactionScript { private get; set; }
        [Inject] public AddTwitterDisplayNameTransactionScript AddTwitterDisplayNameTransactionScript { private get; set; }
        [Inject] public DeleteAuthenticationMethodTransactionScript DeleteAuthenticationMethodTransactionScript { private get; set; }
        [Inject] public GetUserCountTransactionScript GetUserCountTransactionScript { private get; set; }
        [Inject] public GetUserTransactionScript GetUserTransactionScript { private get; set; }
        [Inject] public GetAllUsersTransactionScript GetAllUsersTransactionScript { private get; set; }

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

        public UserDTO AuthenticateWithGoogle(string googleUserId, string email)
        {
            return Run(context => AuthenticateWithGoogleTransactionScript
                .AuthenticateWithGoogle(
                    context,
                    googleUserId, 
                    email));
        }

        public UserDTO AuthenticateWithFacebook(string facebookUserId, string email)
        {
            return Run(context => AuthenticateWithFacebookTransactionScript
                .AuthenticateWithFacebook(
                    facebookUserId,
                    email));
        }

        public UserDTO AuthenticateWithTwitterDisplayName(string twitterDisplayName)
        {
            return Run(context => AuthenticateWithTwitterTransactionScript
                .AuthenticateWithTwitter(twitterDisplayName));
        }

        public UserDTO AddEmailAndPassword(int userId, string email, string password)
        {
            return Run(context => AddEmailAndPasswordTransactionScript.AddEmailAndPassword(userId, email, password));
        }

        public UserDTO AddGoogleUserId(int userId, string googleUserId, string email)
        {
            return Run(context => AddGoogleUserIdTransactionScript.AddGoogleUserId(userId, googleUserId, email));
        }

        public UserDTO AddFacebookUserId(int userId, string facebookUserId, string email)
        {
            return Run(context => AddFacebookUserIdTransactionScript.AddFacebookUserId(userId, facebookUserId, email));
        }

        public UserDTO AddTwitterDisplayName(int userId, string twitterDisplayName)
        {
            return Run(context => AddTwitterDisplayNameTransactionScript.AddTwitterDisplayName(userId, twitterDisplayName));
        }

        public UserDTO DeleteAuthenticationMethod(int userId, int authenticationMethodId)
        {
            return Run(context => DeleteAuthenticationMethodTransactionScript.DeleteAuthenticationMethod(userId, authenticationMethodId));
        }

        public IList<UserDTO> GetAllUsers()
        {
            return Run(context => GetAllUsersTransactionScript.GetAllUsers());
        }

        public int GetUserCount()
        {
            return Run(context => GetUserCountTransactionScript.GetUserCount(context));
        }

        public UserDTO GetUser(int userId)
        {
            return Run(context => GetUserTransactionScript.GetUser(userId));
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
