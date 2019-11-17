using System.ComponentModel.DataAnnotations;

namespace ComicsLibrary.Common.Models
{
    public enum SearchOrder
    {
        [Display(Name = "Title (ascending)")]
        TitleAscending = 1,
        [Display(Name = "Start Year (descending)")]
        StartYearDescending = 2
    }
}