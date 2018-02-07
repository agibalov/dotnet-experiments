using System;

namespace EntityFrameworkExperiment.DTO
{
    public class CommentDTO
    {
        public int CommentId { get; set; }
        public string CommentText { get; set; }
        public UserDTO Author { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? ModifiedAt { get; set; }
    }
}