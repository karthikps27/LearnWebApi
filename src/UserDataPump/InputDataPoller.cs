using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UserDataPump.Models;

namespace UserDataPump.Framework
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

                    await AddOrUpdateEntity(bookItemsDbContext, jsonInputReader.GetAllDataFromJsonFile().First());

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

        private async Task AddOrUpdateEntity(BookItemsDbContext bookItemsDbContext, BookItem bookItem)
        {            
            try
            {
                var bookData = (from book in bookItemsDbContext.BookItems where book.Id == bookItem.Id select book).ToList().First();
                bookData = bookItem;
                await bookItemsDbContext.SaveChangesAsync();
            }
            catch(InvalidOperationException exception) when (exception.Message.Contains("Sequence contains no elements"))
            {
                bookItemsDbContext.BookItems.Add(bookItem);
                await bookItemsDbContext.SaveChangesAsync();
            }                    
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
