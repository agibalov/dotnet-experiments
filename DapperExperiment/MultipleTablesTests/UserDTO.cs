using System.Collections.Generic;

namespace DapperExperiment.MultipleTablesTests
{
    public class UserDTO
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public IList<PostDTO> Posts { get; set; }
    }
}