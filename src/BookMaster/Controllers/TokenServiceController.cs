using BookMaster.Model;
using BookMaster.Security;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookMaster.Controllers
{
    [Produces("application/json")]
    [Route("api/authtoken")]
    public class TokenServiceController : ControllerBase
    {
        private readonly TokenManagerService _tokenManagerService;
        private readonly ILogger _logger;

        public TokenServiceController(TokenManagerService tokenManagerService, ILogger<TokenServiceController> logger)
        {
            _tokenManagerService = tokenManagerService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<string>> GetNewTokenForUser([FromBody] TokenRequest tokenRequest)
        {
            try
            {
                var authtoken = await _tokenManagerService.GenerateToken(tokenRequest.Username, tokenRequest.Password);
                return StatusCode(StatusCodes.Status200OK, new { AuthToken = authtoken });
            }
            catch (Exception exception)
            {
                _logger.LogError($"Error while generating the token: {exception.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, new { Status = "Error while generating the token", exception.Message });
            }
        }
    }
}
