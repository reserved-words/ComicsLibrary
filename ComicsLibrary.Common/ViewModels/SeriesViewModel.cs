using ComicsLibrary.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ComicsLibrary.Common.ViewModels
{
    public class SeriesViewModel
    {
        private readonly Lazy<Tuple<string, string>> _splitTitle;

        public SeriesViewModel()
        {
            _splitTitle = new Lazy<Tuple<string, string>>(GetSplitTitle);
        }

        public int Id { get; set; }
        public int? MarvelId { get; set; }
        public string Title { get; set; }
        public List<Comic> Comics { get; set; }
        public DateTime LastUpdated { get; set; }

        public List<Comic> Read => Comics.Where(c => c.IsRead).ToList();
        public List<Comic> Unread => Comics.Where(c => !c.IsRead).ToList();


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
    }
}