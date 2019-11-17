using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ComicsLibrary.Common.Models
{
    public class Character
    {
        public int Id { get; set; }
        [Required]
        [StringLength(255)]
        public string Name { get; set; }
        public string ImageUrl { get; set; }
        public int? MarvelId { get; set; }
        public string Url { get; set; }
    }
}