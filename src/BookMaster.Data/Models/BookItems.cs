using System.Collections.Generic;

namespace BookMaster.Data.Models
{
    public class BookItem
    {
        public string Id { get; set; }
        public string Etag { get; set; }
        public string Kind { get; set; }
        public string SelfLink { get; set; }
        public string Title { get; set; }
        public List<string> Authors { get; set; }
        public string Publisher { get; set; }
        public string PublishedDate { get; set; }
        public string Description { get; set; }        
        public long PageCount { get; set; }
        public string PrintType { get; set; }
        public List<string> Categories { get; set; }
        public double AverageRating { get; set; }
        public long RatingsCount { get; set; }
        public string MaturityRating { get; set; }
        public bool AllowAnonLogging { get; set; }
        public string ContentVersion { get; set; }
        public string Language { get; set; }
        public string PreviewLink { get; set; }
        public string InfoLink { get; set; }
        public string CanonicalVolumeLink { get; set; }
    }
}
