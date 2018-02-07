using System;
using System.Collections.Generic;

namespace EntityFrameworkExperiment.DAL.Entities
{
    public class Post
    {
        public int PostId { get; set; }
        public string Text { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? ModifiedAt { get; set; }

        public int UserId { get; set; }
        public User User { get; set; }
        public IList<Comment> Comments { get; set; }
    }
}