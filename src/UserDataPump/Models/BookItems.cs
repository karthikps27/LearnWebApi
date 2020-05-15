using System;
using System.Collections.Generic;
using System.Text;

namespace UserDataPump.Models
{
    public class BookItems
    {
        public string Id { get; set; }
        public string Etag { get; set; }
        public string Kind { get; set; }
        public string Language { get; set; }
        public BookVolumeInfo VolumeInfo { get; set; }
        public BookSaleInfo SaleInfo { get; set; }
        public BookAccessInfo AccessInfo { get; set; }
        public BookSearchInfo SearchInfo { get; set; }
    }
}
