using System.Collections.Generic;

namespace ComicsLibrary.Common.Models
{
    public class BookType
    {
        public int ID { get; set; }
        public string Name { get; set; }

        public virtual ICollection<HomeBookType> HomeBookTypes { get; set; }

    }
}
