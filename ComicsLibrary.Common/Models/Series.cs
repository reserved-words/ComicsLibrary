using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace ComicsLibrary.Common.Models
{
    public class Series
    {
        private readonly Lazy<Tuple<string, string>> _splitTitle;

        public Series()
        {
            Books = new List<Book>();
            _splitTitle = new Lazy<Tuple<string, string>>(GetSplitTitle);
        }

        public int Id { get; set; }
        public int? SourceId { get; set; }
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


        public virtual ICollection<HomeBookType> HomeBookTypes { get; set; }

        public virtual Source Source { get; set; }

        public virtual ICollection<Book> Books { get; set; }

        public string MainTitle => _splitTitle.Value.Item1;
        public string SubTitle => _splitTitle.Value.Item2;

        private Tuple<string, string> GetSplitTitle()
        {
            if (Title == null)
                return null;

            var parenthesesStartAt = Title.IndexOf("(");
            if (parenthesesStartAt <= 1)
            {
                return new Tuple<string, string>(Title, "");
            }

            return new Tuple<string, string>(
                Title.Substring(0, parenthesesStartAt - 1),
                Title.Substring(parenthesesStartAt));
        }

        public string YearsActive
        {
            get
            {
                if (StartYear == null)
                    return EndYear?.ToString();

                if (EndYear == null || StartYear == EndYear)
                    return StartYear.ToString();

                return $"{StartYear} - {EndYear}";
            }
        }

        public IEnumerable<Book> GetValidBooks()
        {
            if (HomeBookTypes == null || Books == null)
                return new List<Book>();

            var validTypes = HomeBookTypes
                .Where(bt => bt.Enabled)
                .Select(bt => bt.BookTypeId)
                .ToList();

            return Books
                .Where(b => !b.Hidden
                    && b.BookTypeID.HasValue
                    && validTypes.Contains(b.BookTypeID.Value));

        }
    }
}