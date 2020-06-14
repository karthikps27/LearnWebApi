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
using BookDataPump.Migrations;
using Microsoft.Extensions.Logging;
using Amazon.S3;
using System.Net;

namespace BookDataPump.Framework
{
    public class InputDataPoller : IHostedService
    {
        private readonly ILogger _logger;
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly IConfiguration _configuration;
        public InputDataPoller(IServiceScopeFactory serviceScopeFactory, IConfiguration configuration, ILogger<InputDataPoller> logger)
        {
            _serviceScopeFactory = serviceScopeFactory;
            _configuration = configuration;
            _logger = logger;
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
                    string results = await AwsS3Bucket.ListAllFilesInS3BucketAsync();
                    _logger.LogInformation($"S3 Bucket Results: {results}");

                    //streamReader = new StreamReader(Path.Combine(_configuration.GetSection("ResouceDirectory").Value, _configuration.GetSection("ResourceFilename").Value));                   
                    Stream stream = await AwsS3Bucket.ReadObjectDataAsync();
                    using (streamReader = new StreamReader(stream))
                    {
                        var jsonInputReader = new JsonInputReader(streamReader);
                        await AddOrUpdateEntity(bookItemsDbContext, jsonInputReader.GetAllDataFromJsonFile());
                    }

                    DirectoryService.MoveJsonToDoneDirectory(
                        _configuration.GetSection("ResourceFilename").Value,
                        _configuration.GetSection("ResouceDirectory").Value,
                        _configuration.GetSection("ReadResourceDirectory").Value);

                    _logger.LogInformation("Data from resources file is read and the DB has been updated");
                    HttpStatusCode httpStatusCode = await AwsS3Bucket.DeleteObjectDataAsync();
                    _logger.LogInformation($"Resource file removal from S3 status: {httpStatusCode}");
                }
                catch(FileNotFoundException fe)
                {
                    _logger.LogInformation("Data resources file not found in the location." + fe.Message);
                }
                catch (AmazonS3Exception e)
                {
                    _logger.LogError("Error reading file from S3 bucket" + e.Message);
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
