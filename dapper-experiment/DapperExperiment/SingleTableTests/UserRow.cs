namespace DapperExperiment.SingleTableTests
{
    public class UserRow
    {
        public int UserId { get; set; }
        public string UserName { get; set; }

        public override string ToString()
        {
            return string.Format("UserRow{{UserId={0}, UserName={1}}}", UserId, UserName);
        }
    }
}