using System;
using System.Linq;
using EntityFrameworkExperiment.DAL;
using EntityFrameworkExperiment.DAL.Entities;
using EntityFrameworkExperiment.DTO;
using EntityFrameworkExperiment.Exceptions;
using EntityFrameworkExperiment.Mappers;

namespace EntityFrameworkExperiment.TransactionScripts
{
    public class AuthenticateTransactionScript
    {
        private readonly SessionToSessionDTOMapper _sessionToSessionDtoMapper;

        public AuthenticateTransactionScript(SessionToSessionDTOMapper sessionToSessionDtoMapper)
        {
            _sessionToSessionDtoMapper = sessionToSessionDtoMapper;
        }

        public SessionDTO Authenticate(BlogContext context, string userName, string password)
        {
            var user = context.Users.SingleOrDefault(u => u.UserName == userName);
            if (user == null)
            {
                throw new NoSuchUserException();
            }

            if (user.Password != password)
            {
                throw new InvalidPasswordException();
            }

            var session = new Session
                {
                    SessionToken = Guid.NewGuid().ToString(),
                    User = user
                };

            session = context.Sessions.Add(session);
            context.SaveChanges();

            return _sessionToSessionDtoMapper.Map(session);
        }
    }
}