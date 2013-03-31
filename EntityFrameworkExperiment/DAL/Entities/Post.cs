﻿namespace EntityFrameworkExperiment.DAL.Entities
{
    public class Post
    {
        public int PostId { get; set; }
        public string Text { get; set; }

        public int UserId { get; set; }
        public virtual User User { get; set; }
    }
}