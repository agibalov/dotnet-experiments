using Newtonsoft.Json;

namespace EntityFrameworkExperiment2.DAL.Entities
{
    public class Session
    {
        public int SessionId { get; set; }
        public string SessionToken { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }

        public override string ToString()
        {
            return string.Format("Session {0}", JsonConvert.SerializeObject(this, Formatting.Indented));
        }
    }
}