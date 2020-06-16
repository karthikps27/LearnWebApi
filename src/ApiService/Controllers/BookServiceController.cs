using BookDataService.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;

namespace ApiService.Controllers
{
    [Produces("application/json")]
    [Route("api/books")]
    public class BookServiceController : ControllerBase
    {
        private readonly IBookDataFetchService _bookDataFetchService;
        private readonly ILogger _logger;

        public BookServiceController(IBookDataFetchService bookDataFetchService, ILogger<BookServiceController> logger)
        {
            _bookDataFetchService = bookDataFetchService;
            _logger = logger;
        }

        [HttpGet]
        public object GetBookData([FromQuery] string bookId)
        {
            try
            {
                if(bookId == null)
                {
                    _logger.LogInformation($"Request for viewing all books data received");
                    return _bookDataFetchService.GetAllBooksData();
                }
                
                _logger.LogInformation($"Request for viewing bookdata received with bookId: {bookId}");
                return _bookDataFetchService.GetBookData(bookId);                
            }
            catch(InvalidOperationException exception) when (exception.Message.Contains("Sequence contains no elements"))
            {
                _logger.LogError($"Error with Request for viewing bookdata for bookId: {bookId}");
                return StatusCode(StatusCodes.Status204NoContent, new { Status = "Error while fetching user data", exception.Message });                
            }            
        }        
    }
}
