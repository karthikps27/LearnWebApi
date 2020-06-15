using Amazon.S3;
using Amazon.S3.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace BookDataPump.Framework
{
    public static class AwsS3Bucket
    {
        private static AmazonS3Client client;

        public static async Task<HttpStatusCode> DeleteObjectDataAsync(string bucketName, string key)
        {
            client = new AmazonS3Client();
            try
            {
                DeleteObjectRequest deleteRequest = new DeleteObjectRequest
                {
                    BucketName = bucketName,
                    Key = key
                };
                DeleteObjectResponse deleteObjectResponse = await client.DeleteObjectAsync(deleteRequest);
                return deleteObjectResponse.HttpStatusCode;
            }
            catch (AmazonS3Exception e)
            {
                throw new AmazonS3Exception(e);
            }
        }

        public static async Task<string> ListAllFilesInS3BucketAsync(string bucketName)
        {
            string results = null;
            client = new AmazonS3Client();
            ListObjectsRequest request = new ListObjectsRequest
            {
                BucketName = bucketName
            };

            ListObjectsResponse listObjectsResponse = await client.ListObjectsAsync(request);
            foreach (S3Object entry in listObjectsResponse.S3Objects)
            {
                results += $"key = {entry.Key} size = {entry.Size}";
            }
            return results;
        }

        public static async Task<HttpStatusCode> PutTextFileToS3BucketAsync(string key, string contents, string bucketName)
        {
            PutObjectResponse putObjectResponse;
            try
            {
                using (client = new AmazonS3Client())
                {
                    PutObjectRequest putObjectRequest = new PutObjectRequest
                    {
                        BucketName = bucketName,
                        ContentBody = contents,
                        ContentType = "text/plain",                        
                        Key = key
                    };

                    putObjectResponse = await client.PutObjectAsync(putObjectRequest);
                }
                return putObjectResponse.HttpStatusCode;
            }
            catch (AmazonS3Exception e)
            {
                throw new AmazonS3Exception("Error while putting text file to S3 bucket", e);
            }            
        }

        public static async Task<Stream> ReadObjectDataAsync(string bucketName, string key)
        {
            try
            {
                GetObjectRequest request = new GetObjectRequest
                {
                    BucketName = bucketName,
                    Key = key
                };
                GetObjectResponse response = await client.GetObjectAsync(request);
                return response.ResponseStream;
            }
            catch (AmazonS3Exception e)
            {
                throw new AmazonS3Exception("Error while reading text file from S3 bucket", e);
            }
        }        
    }
}
