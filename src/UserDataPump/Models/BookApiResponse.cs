using System;
using System.Collections.Generic;
using System.Text;

namespace UserDataPump.Models
{
    public class BookApiResponse
    {
        public string Kind { get; set; }
        public int TotalItems { get; set; }
        public List<BookItems> Items { get; set; }
    }
}
