using BookMaster.Service;
using BookMaster.Service.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace ApiService.Controllers
{
    [Produces("application/json")]
    [Route("api/books")]
    public class BookServiceController : ControllerBase
    {
        private readonly BookManagerService _bookManagerService;
        private readonly ILogger _logger;

        public BookServiceController(BookManagerService bookManagerService, ILogger<BookServiceController> logger)
        {
            _bookManagerService = bookManagerService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<Book>> GetBookData([FromQuery] string bookId)
        {
            try
            {
                if(bookId == null)
                {
                    _logger.LogInformation($"Request for viewing all books data received");
                    var all = await _bookManagerService.GetAllBooksData();
                    foreach (var item in all)
                    {
                        // AsQueryable is for deferred execution of the query.
                    }
                    var mybook = all.SingleOrDefault(x => x.Id == "1");
                }
                
                _logger.LogInformation($"Request for viewing bookdata received with bookId: {bookId}");
                return await _bookManagerService.GetBookData(bookId);                
            }
            catch(InvalidOperationException exception) when (exception.Message.Contains("Sequence contains no elements"))
            {
                _logger.LogError($"Error with Request for viewing bookdata for bookId: {bookId}");
                return StatusCode(StatusCodes.Status204NoContent, new { Status = "Error while fetching user data", exception.Message });
            }            
        }        
    }
}
