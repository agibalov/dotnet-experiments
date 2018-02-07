using System.Collections.Generic;

namespace DapperExperiment.MultipleTablesTests.Service.DTO
{
    public class UserDTO
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public IList<PostDTO> Posts { get; set; }
    }
}