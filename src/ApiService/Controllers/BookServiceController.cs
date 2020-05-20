using BookDataService.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiService.Controllers
{
    [Produces("application/json")]
    [Route("api/books")]
    public class BookServiceController : ControllerBase
    {
        private readonly IBookDataFetchService _bookDataFetchService;

        public BookServiceController(IBookDataFetchService bookDataFetchService)
        {
            _bookDataFetchService = bookDataFetchService;
        }

        [HttpGet]
        public object GetBookData([FromQuery] string bookId)
        {
            try
            {
                return _bookDataFetchService.GetBookData(bookId);
            }
            catch(InvalidOperationException exception) when (exception.Message.Contains("Sequence contains no elements"))
            {
                return StatusCode(StatusCodes.Status204NoContent);
            }
            
        }
    }
}
