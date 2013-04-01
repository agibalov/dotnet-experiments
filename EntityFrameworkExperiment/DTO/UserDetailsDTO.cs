using System;
using System.Collections.Generic;

namespace EntityFrameworkExperiment.DTO
{
    public class UserDetailsDTO
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public DateTime RegisteredAt { get; set; }
        public int NumberOfPosts { get; set; }
        public IList<PostDTO> RecentPosts { get; set; }
    }
}