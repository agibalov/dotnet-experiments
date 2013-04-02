using System;
using System.Collections.Generic;

namespace EntityFrameworkExperiment.DAL.Entities
{
    public class User
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? ModifiedAt { get; set; }

        public virtual IList<Session> Sessions { get; set; }
        public virtual IList<Post> Posts { get; set; }
        public virtual IList<Comment> Comments { get; set; }
    }
}