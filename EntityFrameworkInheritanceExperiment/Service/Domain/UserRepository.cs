using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using EntityFrameworkInheritanceExperiment.DAL;
using EntityFrameworkInheritanceExperiment.DAL.Entities;
using EntityFrameworkInheritanceExperiment.Service.Exceptions;

namespace EntityFrameworkInheritanceExperiment.Service.Domain
{  
    public class UserRepository
    {
        private readonly UserContext _context;

        public UserRepository(UserContext context)
        {
            _context = context;
        }

        public IList<DDDUser> GetAllUsers()
        {
            var users = _context.Users
                .Include(u => u.AuthenticationMethods)
                .Include(u => u.EmailAddresses)
                .ToList();

            return users
                .Select(u => new DDDUser(_context, u))
                .ToList();
        }

        public DDDUser FindUserByIdOrThrow(int userId)
        {
            var user = _context.Users
                .Include(u => u.AuthenticationMethods)
                .Include(u => u.EmailAddresses)
                .SingleOrDefault(u => u.UserId == userId);

            if (user == null)
            {
                throw new NoSuchUserException();
            }

            return new DDDUser(_context, user);
        }

        public DDDUser FindUserByEmail(string email)
        {
            var emailAddress = _context.EmailAddresses
                .Include(e => e.User)
                .SingleOrDefault(x => x.Email == email);
            if (emailAddress == null)
            {
                return null;
            }

            return new DDDUser(_context, emailAddress.User);
        }

        public DDDUser FindUserByGoogleUserId(string googleUserId)
        {
            var googleAuthMethod = _context.AuthenticationMethods
                .OfType<GoogleAuthenticationMethod>()
                .Include(am => am.User)
                .SingleOrDefault(x => x.GoogleUserId == googleUserId);
            if (googleAuthMethod == null)
            {
                return null;
            }

            return new DDDUser(_context, googleAuthMethod.User);
        }

        public DDDUser FindUserByFacebookUserId(string facebookUserId)
        {
            var facebookAuthMethod = _context.AuthenticationMethods
                .OfType<FacebookAuthenticationMethod>()
                .Include(am => am.User)
                .SingleOrDefault(x => x.FacebookUserId == facebookUserId);
            if (facebookAuthMethod == null)
            {
                return null;
            }

            return new DDDUser(_context, facebookAuthMethod.User);
        }

        public DDDUser FindUserByTwitterUserId(string twitterUserId)
        {
            var twitterAuthMethod = _context.AuthenticationMethods
                .OfType<TwitterAuthenticationMethod>()
                .Include(am => am.User)
                .SingleOrDefault(x => x.TwitterUserId == twitterUserId);
            if (twitterAuthMethod == null)
            {
                return null;
            }

            return new DDDUser(_context, twitterAuthMethod.User);
        }
    }
}