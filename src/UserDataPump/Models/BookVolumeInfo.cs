using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace UserDataPump.Models
{
    public class BookVolumeInfo
    {
        [Key]
        public string Title { get; set; }
        public List<string> Authors { get; set; }
        public string Publisher { get; set; }
        public string PublishedDate { get; set; }
        public string Description { get; set; }
        public virtual List<IndustryIdentifier> IndustryIdentifiers { get; set; }
        public virtual ReadingMode ReadingModes { get; set; }
        public long PageCount { get; set; }
        public string PrintType { get; set; }
        public List<string> Categories { get; set; }
        public double AverageRating { get; set; }
        public long RatingsCount { get; set; }
        public string MaturityRating { get; set; }
        public bool AllowAnonLogging { get; set; }
        public string ContentVersion { get; set; }
        public virtual ImageLink ImageLinks { get; set; }
        public string Language { get; set; }
        public string PreviewLink { get; set; }
        public string InfoLink { get; set; }
        public string CanonicalVolumeLink { get; set; }
    }

    public class ImageLink
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string SmallThumbnail { get; set; }
        public string Thumbnail { get; set; }
    }

    public class IndustryIdentifier
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Type { get; set; }
        public string Identifier { get; set; }
    }

    public class ReadingMode
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public bool Text { get; set; }
        public bool Image { get; set; }
    }
}
