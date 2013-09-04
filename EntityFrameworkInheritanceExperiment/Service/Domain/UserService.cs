using System;
using System.Collections.Generic;
using System.Linq;
using EntityFrameworkInheritanceExperiment.DAL;
using EntityFrameworkInheritanceExperiment.Service.Exceptions;

namespace EntityFrameworkInheritanceExperiment.Service.Domain
{
    public class UserService
    {
        private readonly UserContext _context;
        private readonly UserFactory _userFactory;
        private readonly UserRepository _userRepository;

        public UserService(
            UserContext context, 
            UserFactory userFactory, 
            UserRepository userRepository)
        {
            _context = context;
            _userFactory = userFactory;
            _userRepository = userRepository;
        }

        public DDDUser AddEmail(int userId, string email)
        {
            var user = _userRepository.FindUserByIdOrThrow(userId);
            user.AddEmail(email);
            _context.SaveChanges();

            return user;
        }

        public DDDUser AddFacebookUserId(int userId, string facebookUserId, string email)
        {
            var user = _userRepository.FindUserByIdOrThrow(userId);

            var somebodyWhoAlreadyHasThisFacebookUserId = _userRepository.FindUserByFacebookUserId(facebookUserId);
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

            var somebodyWhoAlreadyHasThisEmail = _userRepository.FindUserByEmail(email);
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

            _context.SaveChanges();

            return user;
        }

        public DDDUser AddGoogleUserId(int userId, string googleUserId, string email)
        {
            var user = _userRepository.FindUserByIdOrThrow(userId);

            var somebodyWhoAlreadyHasThisGoogleUserId = _userRepository.FindUserByGoogleUserId(googleUserId);
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

            var somebodyWhoAlreadyHasThisEmail = _userRepository.FindUserByEmail(email);
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

            _context.SaveChanges();

            return user;
        }

        public DDDUser AddTwitterDisplayName(int userId, string twitterUserId, string twitterDisplayName)
        {
            var user = _userRepository.FindUserByIdOrThrow(userId);

            var somebodyWhoAlreadyHasThisTwitterDisplayName = _userRepository.FindUserByTwitterUserId(twitterUserId);
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

            _context.SaveChanges();

            return user;
        }

        public DDDUser AuthenticateWithFacebook(string facebookUserId, string email)
        {
            var user = _userRepository.FindUserByFacebookUserId(facebookUserId);
            if (user == null)
            {
                user = _userRepository.FindUserByEmail(email);
                if (user == null)
                {
                    user = _userFactory.MakeUser();
                    user.AddEmail(email);
                    user.AddFacebook(facebookUserId);
                    _context.SaveChanges();
                }
                else
                {
                    user.AddFacebook(facebookUserId);
                    _context.SaveChanges();
                }
            }
            else
            {
                var isNewEmailAddress = !_context.EmailAddresses
                    .Any(e => e.UserId == user.UserId && e.Email == email);
                if (isNewEmailAddress)
                {
                    user.AddEmail(email);
                    _context.SaveChanges();
                }
            }

            return user;
        }

        public DDDUser AuthenticateWithGoogle(string googleUserId, string email)
        {
            var user = _userRepository.FindUserByGoogleUserId(googleUserId);
            if (user == null)
            {
                user = _userRepository.FindUserByEmail(email);
                if (user == null)
                {
                    user = _userFactory.MakeUser();
                    user.AddEmail(email);
                    user.AddGoogle(googleUserId);
                    _context.SaveChanges();
                }
                else
                {
                    user.AddGoogle(googleUserId);
                    _context.SaveChanges();
                }
            }
            else
            {
                var isNewEmailAddress = !_context.EmailAddresses
                    .Any(e => e.UserId == user.UserId && e.Email == email);
                if (isNewEmailAddress)
                {
                    user.AddEmail(email);
                    _context.SaveChanges();
                }
            }

            return user;
        }

        public DDDUser AuthenticateWithTwitter(string twitterUserId, string twitterDisplayName)
        {
            var user = _userRepository.FindUserByTwitterUserId(twitterUserId);
            if (user == null)
            {
                user = _userFactory.MakeUser();
                user.AddTwitter(twitterUserId, twitterDisplayName);
            }

            _context.SaveChanges();

            return user;
        }

        public DDDUser DeleteAuthenticationMethod(int userId, int authenticationMethodId)
        {
            throw new NotImplementedException();
        }

        public IList<DDDUser> GetAllUsers()
        {
            return _userRepository.GetAllUsers();
        }

        public int GetUserCount()
        {
            return _userRepository.GetAllUsers().Count;
        }

        public DDDUser GetUser(int userId)
        {
            throw new NotImplementedException();
        }

        public DDDUser SignInWithEmailAndPassword(string email, string password)
        {
            var user = _userRepository.FindUserByEmail(email);
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

        public DDDUser SignUpWithEmailAndPassword(string email, string password)
        {
            var user = _userRepository.FindUserByEmail(email);
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
                user = _userFactory.MakeUser();
                user.AddEmail(email);
                user.AddPassword(password);
            }

            _context.SaveChanges();

            return user;
        }
    }
}