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

        public DDDUser AddFacebook(int userId, string facebookUserId, string email)
        {
            var user = _userRepository.FindUserByIdOrThrow(userId);
            user.AddFacebook(facebookUserId);
            user.AddEmail(email);

            _context.SaveChanges();

            return user;
        }

        public DDDUser AddGoogle(int userId, string googleUserId, string email)
        {
            var user = _userRepository.FindUserByIdOrThrow(userId);
            user.AddGoogle(googleUserId);
            user.AddEmail(email);

            _context.SaveChanges();

            return user;
        }

        public DDDUser AddTwitter(int userId, string twitterUserId, string twitterDisplayName)
        {
            var user = _userRepository.FindUserByIdOrThrow(userId);
            user.AddTwitter(twitterUserId, twitterDisplayName);

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
                }
                else
                {
                    user.AddFacebook(facebookUserId);
                }
            }
            else
            {
                user.AddEmail(email);
            }

            _context.SaveChanges();

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
                }
                else
                {
                    user.AddGoogle(googleUserId);
                }
            }
            else
            {
                user.AddEmail(email);
            }

            _context.SaveChanges();

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
            var user = _userRepository.FindUserByEmailOrThrow(email);
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