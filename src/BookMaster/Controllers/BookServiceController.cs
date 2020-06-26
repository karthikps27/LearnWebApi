using BookDataService.Service;
using BookMaster.Model;
using BookMaster.Service;
using BookMaster.Service.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
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
        private readonly S3FileProcessorService _s3FileProcessorService;

        public BookServiceController(BookManagerService bookManagerService, S3FileProcessorService s3FileProcessorService, ILogger<BookServiceController> logger)
        {
            _bookManagerService = bookManagerService;
            _logger = logger;
            _s3FileProcessorService = s3FileProcessorService;
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

        [HttpPost]
        public async Task<ActionResult> AddBookData([FromForm] ResourceFileRequest bookDataRequest)
        {
            Stream stream = null;
            try
            {
                Request.Headers.TryGetValue("auth_key", out var authKey);
                if (bookDataRequest.JsonFile.Length > 0 && authKey == "jfklsjafiowuerjlkfujsoirjlk")
                {
                    stream = bookDataRequest.JsonFile.OpenReadStream();
                    using (var streamReader = new StreamReader(stream))
                    {
                        var fileContent = streamReader.ReadToEnd();
                        await _s3FileProcessorService.AddBookData(bookDataRequest.Name, fileContent);
                    }                    
                    return StatusCode(StatusCodes.Status200OK, new { Status = "Data from the file successfully processed" });                    
                }
                return StatusCode(StatusCodes.Status500InternalServerError, new { Status = "Bad request"});
            }
            catch(Exception exception)
            {
                _logger.LogError($"Error while processing data from the file: {exception.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, new { Status = "Error while process data from the file", exception.Message });
            }
            finally
            {
                if(stream != null)
                {
                    stream.Close();
                }                
            }
        }
    }
}
