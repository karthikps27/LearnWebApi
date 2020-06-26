using BookMaster.Data.Framework;
using BookMaster.Data.Models;
using BookMaster.Data.Repository;
using BookMaster.Data.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading.Tasks;

namespace BookDataService.Service
{
    public class S3FileProcessorService
    {
        private readonly AwsS3Bucket _awsS3Bucket;
        private readonly BookRepository _bookRepository;
        private readonly ILogger _logger;

        public S3FileProcessorService(BookRepository bookRepository, AwsS3Bucket awsS3Bucket, ILogger<S3FileProcessorService> logger)
        {
            _awsS3Bucket = awsS3Bucket;
            _bookRepository = bookRepository;
            _logger = logger;
        }
        public async Task AddBookData(String filename, String fileContent)
        {
            HttpStatusCode moveFileStatusCode = await _awsS3Bucket.PutTextFileToS3BucketAsync(filename, fileContent, S3Settings.S3SourceBucketName);
            _logger.LogInformation($"New book data uploaded to S3: {moveFileStatusCode}");
        }

        public async Task ProcessFile(string s3Filename)
        {
            using Stream stream = await _awsS3Bucket.ReadObjectDataAsync(S3Settings.S3SourceBucketName, s3Filename);
            using var streamReader = new StreamReader(stream);
            string bookDataJson = streamReader.ReadToEnd();
            await UpsertBooks(JsonConvert.DeserializeObject<BookApiResponse>(bookDataJson).Items);

            await MoveFileToArchive(s3Filename, bookDataJson);
        }

        public async Task<IEnumerable<string>> GetAllFiles()
        {
            return await _awsS3Bucket.ListAllFilesInS3BucketAsync(S3Settings.S3SourceBucketName);
        }

        private async Task MoveFileToArchive(string originalFilename, string bookDataJson)
        {
            var archiveFilename = $"{originalFilename}-{Guid.NewGuid()}";
            HttpStatusCode moveFileStatusCode = await _awsS3Bucket.PutTextFileToS3BucketAsync(archiveFilename, bookDataJson, S3Settings.S3ArchiveBucketName);
            _logger.LogInformation($"Data from resources file is read and the DB has been updated: {moveFileStatusCode}");

            HttpStatusCode deleteFileStatusCode = await _awsS3Bucket.DeleteObjectDataAsync(S3Settings.S3SourceBucketName, originalFilename);
            _logger.LogInformation($"Resource file removal from S3 status: {deleteFileStatusCode}");
        }

        private async Task<bool> UpsertBooks(List<BookItem> bookItems)
        {
            foreach (var bookItem in bookItems)
            {
                if (!await _bookRepository.UpsertBook(bookItem))
                    return false;
            }
            return true;
        }        
    }
}
