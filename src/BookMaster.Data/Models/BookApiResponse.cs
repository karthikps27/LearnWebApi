using System.Collections.Generic;

namespace BookMaster.Data.Models
{
    public class BookApiResponse
    {
        public string Kind { get; set; }
        public int TotalItems { get; set; }
        public List<BookItem> Items { get; set; }
    }
}
