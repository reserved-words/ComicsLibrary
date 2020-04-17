using System;
using System.Collections.Generic;
using System.Text;

namespace ComicsLibrary.Common.Models
{
    public class SourceOption
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public List<SortOption> SortOptions { get; set; }
    }
}
