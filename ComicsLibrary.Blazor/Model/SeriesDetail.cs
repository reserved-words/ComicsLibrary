using ComicsLibrary.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ComicsLibrary.Blazor.Model
{
    public class SeriesDetail
    {
        public int Id { get; set; }

        public Series Series { get; set; }

        public BookList[] BookLists { get; set; }
    }
}
