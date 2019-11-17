using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ComicsLibrary.Common.Models
{
    public class Series
    {
        private readonly Lazy<Tuple<string, string>> _splitTitle;

        public Series()
        {
            Comics = new List<Comic>();
            _splitTitle = new Lazy<Tuple<string, string>>(GetSplitTitle);
        }

        public int Id { get; set; }
        [Required]
        [StringLength(255)]
        public string Title { get; set; }
        public string ImageUrl { get; set; }
        public int? MarvelId { get; set; }
        public string Url { get; set; }
        public int? Order { get; set; }
        public int? StartYear { get; set; }
        public int? EndYear { get; set; }
        public string Type { get; set; }
        public string Characters { get; set; }
        public DateTime LastUpdated { get; set; }
        public bool IsFinished { get; set; }
        public bool Abandoned { get; set; }

        public virtual ICollection<Comic> Comics { get; set; }

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
    }
}