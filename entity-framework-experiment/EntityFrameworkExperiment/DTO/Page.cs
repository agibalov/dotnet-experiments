using System.Collections.Generic;

namespace EntityFrameworkExperiment.DTO
{
    public class Page<T>
    {
        public int TotalItemCount { get; set; }
        public IList<T> Items { get; set; }
    }
}