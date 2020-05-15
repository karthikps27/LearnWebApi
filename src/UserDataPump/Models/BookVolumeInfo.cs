using System;
using System.Collections.Generic;
using System.Text;

namespace UserDataPump.Models
{
    public class BookVolumeInfo
    {
        public string Title { get; set; }
        public string[] Authors { get; set; }
        public string Publisher { get; set; }
        public string PublishedDate { get; set; }
        public string Description { get; set; }
        public IndustryIdentifier[] IndustryIdentifiers { get; set; }
        public ReadingModes ReadingModes { get; set; }
        public long PageCount { get; set; }
        public string PrintType { get; set; }
        public string[] Categories { get; set; }
        public double AverageRating { get; set; }
        public long RatingsCount { get; set; }
        public string MaturityRating { get; set; }
        public bool AllowAnonLogging { get; set; }
        public string ContentVersion { get; set; }
        public ImageLinks ImageLinks { get; set; }
        public string Language { get; set; }
        public Uri PreviewLink { get; set; }
        public Uri InfoLink { get; set; }
        public Uri CanonicalVolumeLink { get; set; }
    }

    public partial class ImageLinks
    {
        public Uri SmallThumbnail { get; set; }
        public Uri Thumbnail { get; set; }
    }

    public partial class IndustryIdentifier
    {
        public string Type { get; set; }
        public string Identifier { get; set; }
    }

    public partial class ReadingModes
    {
        public bool Text { get; set; }
        public bool Image { get; set; }
    }
}
