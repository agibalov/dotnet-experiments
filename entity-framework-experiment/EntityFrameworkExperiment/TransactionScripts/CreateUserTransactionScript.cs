using System;
using System.Linq;
using EntityFrameworkExperiment.DAL;
using EntityFrameworkExperiment.DAL.Entities;
using EntityFrameworkExperiment.DTO;
using EntityFrameworkExperiment.Exceptions;
using EntityFrameworkExperiment.Mappers;

namespace EntityFrameworkExperiment.TransactionScripts
{
    public class CreateUserTransactionScript
    {
        private readonly UserToUserDTOMapper _userToUserDtoMapper;

        public CreateUserTransactionScript(UserToUserDTOMapper userToUserDtoMapper)
        {
            _userToUserDtoMapper = userToUserDtoMapper;
        }

        public UserDTO CreateUser(BlogContext context, string userName, string password)
        {
            var existingUser = context.Users.SingleOrDefault(u => u.UserName == userName);
            if (existingUser != null)
            {
                throw new UserNameAlreadyRegisteredException();
            }

            var user = new User
                {
                    UserName = userName,
                    Password = password,
                    CreatedAt = DateTime.UtcNow,
                    ModifiedAt = null
                };
            user = context.Users.Add(user);
            context.SaveChanges();

            return _userToUserDtoMapper.Map(user);
        }
    }
}