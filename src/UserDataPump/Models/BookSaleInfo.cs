using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace UserDataPump.Models
{
    public class BookSaleInfo
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int SaleId { get; set; }
        public string Country { get; set; }
        public string Saleability { get; set; }
        public bool IsEbook { get; set; }
        public virtual BookAccessInfoListPrice ListPrice { get; set; }
        public virtual BookAccessInfoListPrice RetailPrice { get; set; }
        public string BuyLink { get; set; }
        public virtual List<Offer> Offers { get; set; }
    }

    public class BookAccessInfoListPrice
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public double Amount { get; set; }
        public string CurrencyCode { get; set; }
    }

    public class Offer
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public long FinskyOfferType { get; set; }
        public virtual OfferListPrice ListPrice { get; set; }
        public virtual OfferListPrice RetailPrice { get; set; }
    }

    public class OfferListPrice
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public long AmountInMicros { get; set; }
        public string CurrencyCode { get; set; }
    }
}
