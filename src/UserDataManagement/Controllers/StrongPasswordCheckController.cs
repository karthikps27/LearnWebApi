using LearnWebApi.Models;
using LearnWebApi.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LearnWebApi.Controllers
{
    [Produces("application/json")]
    [Route("api/users")]
    public class StrongPasswordCheckController : ControllerBase
    {
        private readonly IStrongPasswordCheckService _strongPasswordCheckService;
        public StrongPasswordCheckController(IStrongPasswordCheckService strongPasswordCheckService)
        {
            _strongPasswordCheckService = strongPasswordCheckService;
        }

        [HttpPut]
        public ActionResult<object> AddUserData([FromBody] UserData requestContent)
        {
            bool strength = _strongPasswordCheckService.CheckPasswordStrength(requestContent.Id, requestContent.Password);

            return StatusCode(StatusCodes.Status200OK, new { Strength = strength});
        }

        [HttpGet]
        public async Task<ActionResult<object>> GetUserData([FromQuery] string username)
        {
            var userdata = await _strongPasswordCheckService.GetUserData(username);

            return new ObjectResult(userdata);
        }

        /*
         * Idea is to accept a string and check whether it is a strong password or not. Problem link is here: https://www.hackerrank.com/challenges/strong-password/problem
         * 
         * Extended idea would be to keep adding the last 10 passwords in an arraylist and have APIs that would add/remove/retrieve the passwords
         * arraylist should be created using singleton object and add/remove operation should be concurrency controlled.
         * 2nd version would be to store the passwords in an inmemory db
         * 3rd  version is to store JSON objects both in array list and inmemory db and apply caching mechanism.
         * 4th would to be deploy to aws and connect and do the same with redis and DynDB
         */
    }
}
