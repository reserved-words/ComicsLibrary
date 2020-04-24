using System;

namespace ComicsLibrary.Common
{
    public class BookUpdate
    {
		public string BookTypeName { get; set; }
		public int? SourceItemID { get; set; }
		public string Title { get; set; }
		public double? Number { get; set; }
		public string ImageUrl { get; set; }
		public string ReadUrl { get; set; }
		public string Creators { get; set; }
		public DateTimeOffset? OnSaleDate { get; set; }
	}
}
