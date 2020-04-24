using System.Collections.Generic;

namespace ComicsLibrary.Common.Models
{
    public class SourceOption
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public List<SortOption> SortOptions { get; set; }
    }
}
