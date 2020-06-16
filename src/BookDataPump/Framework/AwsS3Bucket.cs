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
    public class AwsS3Bucket
    {
        private readonly AmazonS3Client _client;

        public AwsS3Bucket() 
        {
            _client = new AmazonS3Client();
        }

        public async Task<HttpStatusCode> DeleteObjectDataAsync(string bucketName, string key)
        {            
            try
            {
                DeleteObjectRequest deleteRequest = new DeleteObjectRequest
                {
                    BucketName = bucketName,
                    Key = key
                };
                DeleteObjectResponse deleteObjectResponse = await _client.DeleteObjectAsync(deleteRequest);
                return deleteObjectResponse.HttpStatusCode;
            }
            catch (AmazonS3Exception e)
            {
                throw new AmazonS3Exception(e);
            }
        }

        public async Task<string> ListAllFilesInS3BucketAsync(string bucketName)
        {
            string results = null;
            ListObjectsRequest request = new ListObjectsRequest
            {
                BucketName = bucketName
            };

            ListObjectsResponse listObjectsResponse = await _client.ListObjectsAsync(request);
            foreach (S3Object entry in listObjectsResponse.S3Objects)
            {
                results += $"key = {entry.Key} size = {entry.Size}";
            }
            return results;
        }

        public async Task<HttpStatusCode> PutTextFileToS3BucketAsync(string key, string contents, string bucketName)
        {
            PutObjectResponse putObjectResponse;
            try
            {
                PutObjectRequest putObjectRequest = new PutObjectRequest
                {
                    BucketName = bucketName,
                    ContentBody = contents,
                    ContentType = "text/plain",                        
                    Key = key
                };
                putObjectResponse = await _client.PutObjectAsync(putObjectRequest);

                return putObjectResponse.HttpStatusCode;
            }
            catch (AmazonS3Exception e)
            {
                throw new AmazonS3Exception("Error while putting text file to S3 bucket", e);
            }            
        }

        public async Task<Stream> ReadObjectDataAsync(string bucketName, string key)
        {
            try
            {
                GetObjectRequest request = new GetObjectRequest
                {
                    BucketName = bucketName,
                    Key = key
                };
                GetObjectResponse response = await _client.GetObjectAsync(request);
                return response.ResponseStream;
            }
            catch (AmazonS3Exception e)
            {
                throw new AmazonS3Exception("Error while reading text file from S3 bucket", e);
            }
        }        
    }
}
