using BookMaster.Data.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace BookMaster.Data.Repository
{
    public class BookRepository
    {
        private readonly BookItemsDbContext _bookItemsDbContext;
        public BookRepository(BookItemsDbContext bookItemsDbContext)
        {
            _bookItemsDbContext = bookItemsDbContext;
        }

        public async Task<BookItem> GetBookData(string bookId)
        {
            return await _bookItemsDbContext.BookItems.SingleOrDefaultAsync(b => b.Id == bookId);
        }

        public IQueryable<BookItem> GetAllBooksData()
        {
            return _bookItemsDbContext.BookItems.AsQueryable();
        }

        public async Task<bool> UpsertBook(BookItem bookItem)
        {
            try
            {
                var existingBook = await GetBookData(bookItem.Id);
                if (existingBook != null)
                {
                    existingBook = bookItem;
                    await _bookItemsDbContext.SaveChangesAsync();
                }
                else
                {
                    _bookItemsDbContext.BookItems.Add(bookItem);
                    await _bookItemsDbContext.SaveChangesAsync();
                    //_logger.LogInformation("New DB record created from the data resources file");
                }
                return true;
            }
            catch (Exception exception) 
            {
                //_logger.Err("DB records updated from the data resources file");
                return false;
            }
        }
    }
}
