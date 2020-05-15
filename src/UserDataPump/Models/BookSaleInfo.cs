using System;
using System.Collections.Generic;
using System.Text;

namespace UserDataPump.Models
{
    public class BookSaleInfo
    {
        public string Country { get; set; }
        public string Saleability { get; set; }
        public bool IsEbook { get; set; }
        public BookAccessInfoListPrice ListPrice { get; set; }
        public BookAccessInfoListPrice RetailPrice { get; set; }
        public Uri BuyLink { get; set; }
        public Offer[] Offers { get; set; }
    }

    public partial class BookAccessInfoListPrice
    {
        public double Amount { get; set; }
        public string CurrencyCode { get; set; }
    }

    public partial class Offer
    {
        public long FinskyOfferType { get; set; }
        public OfferListPrice ListPrice { get; set; }
        public OfferListPrice RetailPrice { get; set; }
    }

    public partial class OfferListPrice
    {
        public long AmountInMicros { get; set; }
        public string CurrencyCode { get; set; }
    }
}
