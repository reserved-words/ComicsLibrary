using System;
using System.ComponentModel.DataAnnotations;

namespace ComicsLibrary.Common.Models
{
    public class Book
    {
        public int Id { get; set; }
        public int? BookTypeID { get; set; }
        public int SeriesId { get; set; }
        public int? SourceItemID { get; set; }

        [Required]
        [StringLength(255)]
        public string Title { get; set; }
        public double? Number { get; set; }
        public string Creators { get; set; }
        public DateTimeOffset? OnSaleDate { get; set; }

        public string ImageUrl { get; set; }
        public string ReadUrl { get; set; }

        public DateTime DateAdded { get; set; }
        public DateTime? ReadUrlAdded { get; set; }
        public DateTime? DateRead { get; set; }
        public bool Hidden { get; set; }

        public virtual BookType BookType { get; set; }
        public virtual Series Series { get; set; }
    }
}