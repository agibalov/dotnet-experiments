namespace EntityFrameworkExperiment.DTO
{
    public class PostDTO
    {
        public int PostId { get; set; }
        public string PostText { get; set; }
        public UserDTO Author { get; set; }
    }
}