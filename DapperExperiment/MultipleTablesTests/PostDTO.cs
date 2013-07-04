namespace DapperExperiment.MultipleTablesTests
{
    public class PostDTO
    {
        public int PostId { get; set; }
        public string PostText { get; set; }
        public BriefUserDTO User { get; set; }
    }
}