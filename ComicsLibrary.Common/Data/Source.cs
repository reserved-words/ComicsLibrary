﻿using System.ComponentModel.DataAnnotations;

namespace ComicsLibrary.Common.Data
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
