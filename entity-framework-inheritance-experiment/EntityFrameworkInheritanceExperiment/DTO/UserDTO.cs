using System.Collections.Generic;

namespace EntityFrameworkInheritanceExperiment.DTO
{
    public class UserDTO
    {
        public int UserId { get; set; }
        public IList<AuthenticationMethodDTO> AuthenticationMethods { get; set; }
        public IList<EmailAddressDTO> EmailAddresses { get; set; }
    }
}