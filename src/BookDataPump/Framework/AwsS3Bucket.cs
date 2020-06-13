using Amazon.S3;
using Amazon.S3.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookDataPump.Framework
{
    public static class AwsS3Bucket
    {
        private static AmazonS3Client client;
        public static async Task<string> ListAllFilesInS3BucketAsync()
        {
            string results = null;
            client = new AmazonS3Client();
            ListObjectsRequest request = new ListObjectsRequest
            {
                BucketName = "book-data-resources"
            };

            ListObjectsResponse listObjectsResponse = await client.ListObjectsAsync(request);
            foreach (S3Object entry in listObjectsResponse.S3Objects)
            {
                results += $"key = {entry.Key} size = {entry.Size}";
            }
            return results;
        }

        public static async Task<Stream> ReadObjectDataAsync()
        {
            try
            {
                GetObjectRequest request = new GetObjectRequest
                {
                    BucketName = "book-data-resources",
                    Key = "BookResponse.json"
                };
                GetObjectResponse response = await client.GetObjectAsync(request);
                return response.ResponseStream;
            }
            catch (AmazonS3Exception e)
            {
                throw e;
            }
        }
    }
}
