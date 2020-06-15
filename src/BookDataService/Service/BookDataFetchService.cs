using BookDataPump.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookDataService.Service
{
    public interface IBookDataFetchService
    {
        BookItem GetBookData(string bookId);
        List<BookItem> GetAllBooksData();
    }
    public class BookDataFetchService : IBookDataFetchService
    {
        private readonly BookItemsDbContext _bookItemsDbContext;
        public BookDataFetchService(BookItemsDbContext bookItemsDbContext)
        {
            _bookItemsDbContext = bookItemsDbContext;
        }

        public BookItem GetBookData(string bookId)
        {
            var bookData = _bookItemsDbContext.BookItems.Where(b => b.Id == bookId).ToList().First();
            return bookData;
        }

        public List<BookItem> GetAllBooksData()
        {
            return _bookItemsDbContext.BookItems.ToList();
        }
    }
}
