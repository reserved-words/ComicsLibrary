using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ComicsLibrary.Common.Models
{
    public class Source
    {
        public int ID { get; set; }

        [MaxLength(20)]
        public string Name { get; set; }

        [MaxLength(255)]
        public string SeriesUrlFormat { get; set; }

        [MaxLength(255)]
        public string BookUrlFormat { get; set; }

        [MaxLength(255)]
        public string ReadUrlFormat { get; set; }
    }
}
