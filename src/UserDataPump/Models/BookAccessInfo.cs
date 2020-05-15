using System;
using System.Collections.Generic;
using System.Text;

namespace UserDataPump.Models
{
    public partial class BookAccessInfo
    {        
        public string Country { get; set; }
        public string Viewability { get; set; }
        public bool Embeddable { get; set; }
        public bool PublicDomain { get; set; }
        public string TextToSpeechPermission { get; set; }
        public Epub Epub { get; set; }
        public Epub Pdf { get; set; }
        public Uri WebReaderLink { get; set; }
        public string AccessViewStatus { get; set; }
        public bool QuoteSharingAllowed { get; set; }
    }

    public partial class Epub
    {
        public bool IsAvailable { get; set; }
        public Uri AcsTokenLink { get; set; }
    }
}
