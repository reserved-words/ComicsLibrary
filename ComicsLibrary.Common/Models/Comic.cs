using System;
using System.ComponentModel.DataAnnotations;

namespace ComicsLibrary.Common.Models
{
    public class Comic
    {
        public int Id { get; set; }
        [Required]
        [StringLength(255)]
        public string Title { get; set; }
        public string ImageUrl { get; set; }
        public int? MarvelId { get; set; }
        public string ReadUrl { get; set; }
        public string DiamondCode { get; set; }
        public double? IssueNumber { get; set; }
        public string Upc { get; set; }
        public int SeriesId { get; set; }
        public string Creators { get; set; }
        public DateTimeOffset? OnSaleDate { get; set; }
        public bool IsRead { get; set; }
        public DateTime DateAdded { get; set; }
        public DateTime? ReadUrlAdded { get; set; }
        public DateTime? DateRead { get; set; }

        public virtual Series Series { get; set; }
    }
}