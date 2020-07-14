using BookMaster.Data.Repository;
using BookMaster.Service.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BookMaster.Service
{
    public class BookManagerService
    {
        private readonly BookRepository _bookRepository;

        public BookManagerService(BookRepository bookRepository)
        {
            _bookRepository = bookRepository;
        }

        public async Task<List<Book>> GetAllBooksData()
        {
            return await Task.FromResult(new List<Book>());
        }

        public async Task<Book> GetBookData(string bookId)
        {
            var bookEntity = await _bookRepository.GetBookData(bookId);
            var mappedBook = new Book { 
                Id = bookEntity.Id,
                Title = bookEntity.Title
            };
            return mappedBook;
        }
    }
}
