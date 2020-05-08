using System.ComponentModel.DataAnnotations;

namespace ComicsLibrary.Common
{
    public enum SearchOrder
    {
        [Display(Name = "Title (ascending)")]
        TitleAscending = 1,
        [Display(Name = "Start Year (descending)")]
        StartYearDescending = 2
    }
}