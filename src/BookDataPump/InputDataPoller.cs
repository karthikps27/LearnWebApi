using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using BookDataPump.Models;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using Amazon.S3;
using System.Net;
using BookDataPump.Configuration;
using Newtonsoft.Json;

namespace BookDataPump.Framework
{
    public class InputDataPoller : IHostedService
    {
        private readonly ILogger _logger;
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly IConfiguration _configuration;
        private readonly AwsS3Bucket _awsS3Bucket;
        public InputDataPoller(IServiceScopeFactory serviceScopeFactory, IConfiguration configuration, ILogger<InputDataPoller> logger, AwsS3Bucket awsS3Bucket)
        {
            _serviceScopeFactory = serviceScopeFactory;
            _configuration = configuration;
            _logger = logger;
            _awsS3Bucket = awsS3Bucket;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            StreamReader streamReader;
            using var scope = _serviceScopeFactory.CreateScope();            
            while (!cancellationToken.IsCancellationRequested)
            {
                var bookItemsDbContext = scope.ServiceProvider.GetRequiredService<BookItemsDbContext>();
                try
                {
                    string results = await _awsS3Bucket.ListAllFilesInS3BucketAsync(S3Settings.S3SourceBucketName);
                    _logger.LogInformation($"S3 Bucket Results: {results}");

                    Stream stream = await _awsS3Bucket.ReadObjectDataAsync(S3Settings.S3SourceBucketName, S3Settings.InputDataFilename);
                    using (streamReader = new StreamReader(stream))
                    {
                        var jsonInputReader = new JsonInputReader(streamReader);
                        string bookDataJson = jsonInputReader.GetAllDataFromJsonFileSerialized();
                        await AddOrUpdateEntity(bookItemsDbContext, JsonConvert.DeserializeObject<BookApiResponse>(bookDataJson).Items);

                        await _awsS3Bucket.PutTextFileToS3BucketAsync(S3Settings.ArchiveFilename, bookDataJson, S3Settings.S3ArchiveBucketName);
                        _logger.LogInformation("Data from resources file is read and the DB has been updated");

                        HttpStatusCode httpStatusCode = await _awsS3Bucket.DeleteObjectDataAsync(S3Settings.S3SourceBucketName, S3Settings.InputDataFilename);
                        _logger.LogInformation($"Resource file removal from S3 status: {httpStatusCode}");
                    }                    
                }
                catch(FileNotFoundException fe)
                {
                    _logger.LogInformation("Data resource file not found in the location." + fe.Message);
                }
                catch (AmazonS3Exception e)
                {
                    if(e.Message.Contains("Error while reading text file"))
                    {
                        _logger.LogInformation("No input data file in S3");
                    }
                    else
                    {
                        _logger.LogError($"Error accessing S3 bucket. {e.Message} {e.StackTrace}");
                    }
                }
                await Task.Delay(TimeSpan.FromSeconds(10));
            }
        }

        private async Task AddOrUpdateEntity(BookItemsDbContext bookItemsDbContext, List<BookItem> bookItems)
        {
            foreach(var bookItem in bookItems)
            {
                try
                {
                    var bookData = (from book in bookItemsDbContext.BookItems where book.Id == bookItem.Id select book).ToList().First();
                    bookData = bookItem;
                    await bookItemsDbContext.SaveChangesAsync();
                    _logger.LogInformation("New DB record created from the data resources file");
                }
                catch (InvalidOperationException exception) when (exception.Message.Contains("Sequence contains no elements"))
                {
                    bookItemsDbContext.BookItems.Add(bookItem);
                    await bookItemsDbContext.SaveChangesAsync();
                    _logger.LogInformation("DB records updated from the data resources file");
                }
            }
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
