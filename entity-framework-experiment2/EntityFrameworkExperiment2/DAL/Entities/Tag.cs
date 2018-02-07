using System.Collections.Generic;
using Newtonsoft.Json;

namespace EntityFrameworkExperiment2.DAL.Entities
{
    public class Tag
    {
        public int TagId { get; set; }
        public string TagName { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }

        [JsonIgnore]
        public IList<Note> Notes { get; set; }

        public override string ToString()
        {
            return string.Format("Tag {0}", JsonConvert.SerializeObject(this, Formatting.Indented));
        }
    }
}