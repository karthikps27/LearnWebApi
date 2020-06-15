using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace BookDataPump.Configuration
{
    public static class S3Settings
    {
        public static string S3SourceBucketName = "book-data-resources";
        public static string S3ArchiveBucketName = "book-data-archive";
        public static string InputDataFilename = "BookResponse.json";
        public static string ArchiveFilename = $"BookResponse_{Regex.Replace(Convert.ToBase64String(Guid.NewGuid().ToByteArray()), "[/+=]", "")}.json";
    }
}
