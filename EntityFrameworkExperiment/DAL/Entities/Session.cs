﻿namespace EntityFrameworkExperiment.DAL.Entities
{
    public class Session
    {
        public int SessionId { get; set; }
        public string SessionToken { get; set; }
        public int UserId { get; set; }
        public virtual User User { get; set; }
    }
}