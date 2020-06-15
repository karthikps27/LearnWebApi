using System.Collections.Generic;

namespace BookDataPump.Models
{
    public class BookApiResponse
    {
        public string Kind { get; set; }
        public int TotalItems { get; set; }
        public List<BookItem> Items { get; set; }
    }
}
