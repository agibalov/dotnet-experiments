using Newtonsoft.Json;

namespace EntityFrameworkExperiment2.DAL.Entities
{
    public class User
    {
        public int UserId { get; set; }
        public string UserName { get; set; }

        public override string ToString()
        {
            return string.Format("User {0}", JsonConvert.SerializeObject(this, Formatting.Indented));
        }
    }
}