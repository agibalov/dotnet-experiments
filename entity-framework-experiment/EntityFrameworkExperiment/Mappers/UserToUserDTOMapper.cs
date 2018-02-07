using EntityFrameworkExperiment.DAL.Entities;
using EntityFrameworkExperiment.DTO;

namespace EntityFrameworkExperiment.Mappers
{
    public class UserToUserDTOMapper
    {
        public UserDTO Map(User user)
        {
            return new UserDTO
                {
                    UserId = user.UserId,
                    UserName = user.UserName,
                    CreatedAt = user.CreatedAt,
                    ModifiedAt = user.ModifiedAt
                };
        }
    }
}