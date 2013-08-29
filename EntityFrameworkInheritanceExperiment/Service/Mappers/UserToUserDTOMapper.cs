using System.Linq;
using EntityFrameworkInheritanceExperiment.DAL.Entities;
using EntityFrameworkInheritanceExperiment.DTO;
using EntityFrameworkInheritanceExperiment.Service.Configuration;

namespace EntityFrameworkInheritanceExperiment.Service.Mappers
{
    [Mapper]
    public class UserToUserDTOMapper
    {
        private readonly AuthenticationMethodToAuthenticationMethodDTOMapper _authenticationMethodToAuthenticationMethodDtoMapper;

        public UserToUserDTOMapper(AuthenticationMethodToAuthenticationMethodDTOMapper authenticationMethodToAuthenticationMethodDtoMapper)
        {
            _authenticationMethodToAuthenticationMethodDtoMapper = authenticationMethodToAuthenticationMethodDtoMapper;
        }

        public UserDTO MapUserToUserDTO(User user)
        {
            return new UserDTO
                {
                    UserId = user.UserId,
                    AuthenticationMethods = user
                        .AuthenticationMethods.Select(_authenticationMethodToAuthenticationMethodDtoMapper.MapAuthenticationMethodToAuthenticationMethodDTO)
                        .ToList(),
                    EmailAddresses = user
                        .EmailAddresses.Select(emailAddress => new EmailAddressDTO
                            {
                                EmailAddressId = emailAddress.EmailAddressId,
                                Email = emailAddress.Email
                            }).ToList()
                };
        }
    }
}