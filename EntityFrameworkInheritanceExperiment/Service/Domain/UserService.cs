using System;
using System.Collections.Generic;
using System.Linq;
using EntityFrameworkInheritanceExperiment.DAL;
using EntityFrameworkInheritanceExperiment.Service.Configuration;
using EntityFrameworkInheritanceExperiment.Service.Exceptions;

namespace EntityFrameworkInheritanceExperiment.Service.Domain
{
    [Service]
    public class UserService
    {
        private readonly UserFactory _userFactory;
        private readonly UserRepository _userRepository;

        public UserService(UserFactory userFactory, UserRepository userRepository)
        {
            _userFactory = userFactory;
            _userRepository = userRepository;
        }

        public DDDUser AddEmail(UserContext context, int userId, string email)
        {
            var user = _userRepository.FindUserByIdOrThrow(context, userId);

            var somebodyWhoAlreadyHasThisEmail = _userRepository.FindUserByEmail(context, email);
            if (somebodyWhoAlreadyHasThisEmail != null)
            {
                if (somebodyWhoAlreadyHasThisEmail.UserId != user.UserId)
                {
                    throw new EmailAlreadyUsedException();
                }
            }

            user.AddEmail(email);
            context.SaveChanges();

            return user;
        }

        public DDDUser AddFacebookUserId(UserContext context, int userId, string facebookUserId, string email)
        {
            var user = _userRepository.FindUserByIdOrThrow(context, userId);

            var somebodyWhoAlreadyHasThisFacebookUserId = _userRepository.FindUserByFacebookUserId(context, facebookUserId);
            if (somebodyWhoAlreadyHasThisFacebookUserId != null)
            {
                if (somebodyWhoAlreadyHasThisFacebookUserId.UserId != user.UserId)
                {
                    throw new FacebookUserIdAlreadyUsedException();
                }
            }
            else
            {
                user.AddFacebook(facebookUserId);
            }

            var somebodyWhoAlreadyHasThisEmail = _userRepository.FindUserByEmail(context, email);
            if (somebodyWhoAlreadyHasThisEmail != null)
            {
                if (somebodyWhoAlreadyHasThisEmail.UserId != user.UserId)
                {
                    throw new EmailAlreadyUsedException();
                }
            }
            else
            {
                user.AddEmail(email);
            }

            context.SaveChanges();

            return user;
        }

        public DDDUser AddGoogleUserId(UserContext context, int userId, string googleUserId, string email)
        {
            var user = _userRepository.FindUserByIdOrThrow(context, userId);

            var somebodyWhoAlreadyHasThisGoogleUserId = _userRepository.FindUserByGoogleUserId(context, googleUserId);
            if (somebodyWhoAlreadyHasThisGoogleUserId != null)
            {
                if (somebodyWhoAlreadyHasThisGoogleUserId.UserId != user.UserId)
                {
                    throw new GoogleUserIdAlreadyUsedException();
                }
            }
            else
            {
                user.AddGoogle(googleUserId);
            }

            var somebodyWhoAlreadyHasThisEmail = _userRepository.FindUserByEmail(context, email);
            if (somebodyWhoAlreadyHasThisEmail != null)
            {
                if (somebodyWhoAlreadyHasThisEmail.UserId != user.UserId)
                {
                    throw new EmailAlreadyUsedException();
                }
            }
            else
            {
                user.AddEmail(email);
            }

            context.SaveChanges();

            return user;
        }

        public DDDUser AddTwitterDisplayName(UserContext context, int userId, string twitterUserId, string twitterDisplayName)
        {
            var user = _userRepository.FindUserByIdOrThrow(context, userId);

            var somebodyWhoAlreadyHasThisTwitterDisplayName = _userRepository.FindUserByTwitterUserId(context, twitterUserId);
            if (somebodyWhoAlreadyHasThisTwitterDisplayName != null)
            {
                if (somebodyWhoAlreadyHasThisTwitterDisplayName.UserId != user.UserId)
                {
                    throw new TwitterUserIdAlreadyUsedException();
                }
            }
            else
            {
                user.AddTwitter(twitterUserId, twitterDisplayName);
            }

            context.SaveChanges();

            return user;
        }

        public DDDUser AuthenticateWithFacebook(UserContext context, string facebookUserId, string email)
        {
            var user = _userRepository.FindUserByFacebookUserId(context, facebookUserId);
            if (user == null)
            {
                user = _userRepository.FindUserByEmail(context, email);
                if (user == null)
                {
                    user = _userFactory.MakeUser(context);
                    user.AddEmail(email);
                    user.AddFacebook(facebookUserId);
                    context.SaveChanges();
                }
                else
                {
                    user.AddFacebook(facebookUserId);
                    context.SaveChanges();
                }
            }
            else
            {
                var isNewEmailAddress = !context.EmailAddresses
                    .Any(e => e.UserId == user.UserId && e.Email == email);
                if (isNewEmailAddress)
                {
                    user.AddEmail(email);
                    context.SaveChanges();
                }
            }

            return user;
        }

        public DDDUser AuthenticateWithGoogle(UserContext context, string googleUserId, string email)
        {
            var user = _userRepository.FindUserByGoogleUserId(context, googleUserId);
            if (user == null)
            {
                user = _userRepository.FindUserByEmail(context, email);
                if (user == null)
                {
                    user = _userFactory.MakeUser(context);
                    user.AddEmail(email);
                    user.AddGoogle(googleUserId);
                    context.SaveChanges();
                }
                else
                {
                    user.AddGoogle(googleUserId);
                    context.SaveChanges();
                }
            }
            else
            {
                var isNewEmailAddress = !context.EmailAddresses
                    .Any(e => e.UserId == user.UserId && e.Email == email);
                if (isNewEmailAddress)
                {
                    user.AddEmail(email);
                    context.SaveChanges();
                }
            }

            return user;
        }

        public DDDUser AuthenticateWithTwitter(UserContext context, string twitterUserId, string twitterDisplayName)
        {
            var user = _userRepository.FindUserByTwitterUserId(context, twitterUserId);
            if (user == null)
            {
                user = _userFactory.MakeUser(context);
                user.AddTwitter(twitterUserId, twitterDisplayName);
            }

            context.SaveChanges();

            return user;
        }

        public DDDUser DeleteAuthenticationMethod(UserContext context, int userId, int authenticationMethodId)
        {
            throw new NotImplementedException();
        }

        public IList<DDDUser> GetAllUsers(UserContext context)
        {
            return _userRepository.GetAllUsers(context);
        }

        public int GetUserCount(UserContext context)
        {
            var userCount = context.Users.Count();
            return userCount;
        }

        public DDDUser GetUser(UserContext context, int userId)
        {
            throw new NotImplementedException();
        }

        public DDDUser SignInWithEmailAndPassword(UserContext context, string email, string password)
        {
            var user = _userRepository.FindUserByEmail(context, email);
            if (user == null)
            {
                throw new EmailNotRegisteredException();
            }

            var passwordAuthMethods = user.GetPasswords();
            var passwordIsOk = passwordAuthMethods.Any(am => am.Password == password);
            if (!passwordIsOk)
            {
                throw new IncorrectPasswordException();
            }

            return user;
        }

        public DDDUser SignUpWithEmailAndPassword(UserContext context, string email, string password)
        {
            var user = _userRepository.FindUserByEmail(context, email);
            if (user != null)
            {
                var userHasPasswordSet = user.HasPasswordSet();
                if (userHasPasswordSet)
                {
                    throw new EmailAlreadyUsedException();
                }

                user.AddPassword(password);
            }
            else
            {
                user = _userFactory.MakeUser(context);
                user.AddEmail(email);
                user.AddPassword(password);
            }

            context.SaveChanges();

            return user;
        }
    }
}