using System;

namespace EntityFrameworkExperiment.DTO
{
    public class PostDTO
    {
        public int PostId { get; set; }
        public string PostText { get; set; }
        public UserDTO Author { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? ModifiedAt { get; set; }
    }
}