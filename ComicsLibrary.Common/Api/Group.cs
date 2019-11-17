using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComicsLibrary.Common.Api
{
    public class Group
    {
        public Group()
        {

        }

        public Group(string description)
        {
            Description = description;
            Series = new List<Series>();
        }

        public string Description { get; set; }
        public List<Series> Series { get; set; }
    }
}
