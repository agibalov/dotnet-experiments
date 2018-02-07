using EntityFrameworkExperiment.DAL.Entities;
using EntityFrameworkExperiment.DTO;

namespace EntityFrameworkExperiment.Mappers
{
    public class SessionToSessionDTOMapper
    {
        private readonly UserToUserDTOMapper _userToUserDtoMapper;

        public SessionToSessionDTOMapper(UserToUserDTOMapper userToUserDtoMapper)
        {
            _userToUserDtoMapper = userToUserDtoMapper;
        }

        public SessionDTO Map(Session session)
        {
            return new SessionDTO
                {
                    SessionToken = session.SessionToken,
                    User = _userToUserDtoMapper.Map(session.User)
                };
        }
    }
}