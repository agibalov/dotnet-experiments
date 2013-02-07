using System.Collections.Generic;

namespace EntityFrameworkExperiment.DAL.Entities
{
    public class Blog
    {
        public int BlogId { get; set; }
        public string Name { get; set; }

        public virtual IList<Post> Posts { get; set; }

        public override string ToString()
        {
            return string.Format("Blog{{Id={0}, Name={1}}}", BlogId, Name);
        }
    }
}