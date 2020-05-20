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

namespace BookDataPump.Framework
{
    public class InputDataPoller : IHostedService
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly IConfiguration _configuration;
        public InputDataPoller(IServiceScopeFactory serviceScopeFactory, IConfiguration configuration)
        {
            _serviceScopeFactory = serviceScopeFactory;
            _configuration = configuration;
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
                    streamReader = new StreamReader(Path.Combine(_configuration.GetSection("ResouceDirectory").Value, _configuration.GetSection("ResourceFilename").Value));
                    var jsonInputReader = new JsonInputReader(streamReader);

                    await AddOrUpdateEntity(bookItemsDbContext, jsonInputReader.GetAllDataFromJsonFile());

                    streamReader.Close();
                    DirectoryService.MoveJsonToDoneDirectory(
                        _configuration.GetSection("ResourceFilename").Value,
                        _configuration.GetSection("ResouceDirectory").Value,
                        _configuration.GetSection("ReadResourceDirectory").Value);
                }
                catch(FileNotFoundException fe)
                {

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
                }
                catch (InvalidOperationException exception) when (exception.Message.Contains("Sequence contains no elements"))
                {
                    bookItemsDbContext.BookItems.Add(bookItem);
                    await bookItemsDbContext.SaveChangesAsync();
                }
            }                                
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
