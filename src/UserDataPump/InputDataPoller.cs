using Microsoft.EntityFrameworkCore;
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
        public InputDataPoller(IServiceScopeFactory serviceScopeFactory)
        {
            _serviceScopeFactory = serviceScopeFactory;
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
                    streamReader = new StreamReader("D:/projects/Resources/BookResponse.json");
                    var jsonInputReader = new JsonInputReader(streamReader);

                    await AddOrUpdateEntity(bookItemsDbContext, jsonInputReader.GetAllDataFromJsonFile().First());

                    streamReader.Close();
                    DirectoryService.MoveJsonToDoneDirectory(
                        "BookResponse.json",
                        "D:/projects/Resources",
                        "D:/projects/ResourcesReadComplete");
                }
                catch(FileNotFoundException fe)
                {
                    
                }                
                await Task.Delay(TimeSpan.FromSeconds(10));
            }
        }

        private async Task AddOrUpdateEntity(BookItemsDbContext bookItemsDbContext, BookItem bookItem)
        {
            //var bookData = (from book in bookItemsDbContext.BookItems where book.Id == bookItem.Id select book).ToList().First();
            //var bookData = null;
                /* bookItemsDbContext.BookItems
                /*.Include(b => b.AccessInfo)
                    .ThenInclude(b => b.Epub)
                .Include(b => b.AccessInfo.Pdf)
                .Include(b => b.SaleInfo)
                .Include(b => b.SearchInfo)
                .Include(b => b.VolumeInfo)
                .First(b => b.Id == bookItem.Id); */
            if (null == null)
            {
                bookItemsDbContext.BookItems.Add(bookItem);
                await bookItemsDbContext.SaveChangesAsync();
            }
            else
            {
                //bookData.VolumeInfo.Publisher = bookItem.VolumeInfo.Publisher;
                //bookData = bookItem;
                //await bookItemsDbContext.SaveChangesAsync();
            }            
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
