using Amazon.S3;
using Amazon.S3.Model;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace BookMaster.Data.Framework
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

        public async Task<IEnumerable<string>> ListAllFilesInS3BucketAsync(string bucketName)
        {
            var request = new ListObjectsRequest {
                BucketName = bucketName
            };

            ListObjectsResponse listObjectsResponse = await _client.ListObjectsAsync(request);
            var results = listObjectsResponse.S3Objects.Select(x => x.Key);
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
