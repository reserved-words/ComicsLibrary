using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ComicsLibrary.Common.Data
{
    public class Series
    {
        public Series()
        {
            Books = new List<Book>();
        }

        public int Id { get; set; }
        public int? SourceId { get; set; }
        public int? PublisherId { get; set; }
        [Required]
        [StringLength(255)]
        public string Title { get; set; }
        public string ImageUrl { get; set; }
        public int? SourceItemID { get; set; }
        public string Url { get; set; }
        public int? StartYear { get; set; }
        public int? EndYear { get; set; }
        public string Type { get; set; }
        public DateTime LastUpdated { get; set; }
        public bool IsFinished { get; set; }
        public bool Abandoned { get; set; }
        public Shelf? Shelf { get; set; }

        public virtual Source Source { get; set; }
        public virtual Publisher Publisher { get; set; }

        public virtual ICollection<HomeBookType> HomeBookTypes { get; set; }
        public virtual ICollection<Book> Books { get; set; }
    }
}