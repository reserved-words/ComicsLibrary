using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComicsLibrary.Common.Models
{
    public class ApiResult<T>
    {
        public bool Success { get; set; }
        public int Total { get; set; }
        public List<T> Results { get; set; }
    }
}
