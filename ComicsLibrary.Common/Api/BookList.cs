using System;
using System.Collections.Generic;

namespace ComicsLibrary.Common.Api
{
    public class BookList
    {
        public int TypeId { get; set; }
        public string TypeName { get; set; }
        public Comic[] Books { get; set; }
        public int TotalBooks { get; set; }
        public bool Home { get; set; }
    }
}
