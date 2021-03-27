using System.ComponentModel.DataAnnotations;

namespace ComicsLibrary.Common.Data
{
    public class Publisher
    {
        public int Id { get; set; }
        [Required]
        [StringLength(20)]
        public string FullName { get; set; }
        [Required]
        [StringLength(2)]
        public string ShortName { get; set; }
        [Required]
        [StringLength(7)]
        public string Colour { get; set; }
    }
}
