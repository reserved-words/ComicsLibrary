using ComicsLibrary.Common.Models;
using System.ComponentModel.DataAnnotations;

namespace ComicsLibrary.Common.ViewModels
{
    public class CharacterSearchViewModel
    {
        public int Page { get; set; }
        [Display(Name = "Character name starts with:")]
        public string NameStartsWith { get; set; }
        public PagedListViewModel<Character> Results { get; set; }
    }
}
