using System;
using System.Collections.Generic;
using System.Text;

namespace UserDataPump.Models
{
    public class BookItem
    {
        public string Id { get; set; }
        public string Etag { get; set; }
        public string Kind { get; set; }
        public string SelfLink { get; set; }        
        public virtual BookVolumeInfo VolumeInfo { get; set; }
        public virtual BookSaleInfo SaleInfo { get; set; }
        public virtual BookAccessInfo AccessInfo { get; set; }
        public virtual BookSearchInfo SearchInfo { get; set; }
    }
}
