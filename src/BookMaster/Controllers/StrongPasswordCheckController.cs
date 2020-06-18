using LearnWebApi.Models;
using LearnWebApi.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
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
        public async Task<ActionResult<object>> AddUserData([FromBody] UserData requestContent)
        {
            try
            {
                bool strength = await _strongPasswordCheckService.CheckPasswordStrengthAsync(requestContent.Username, requestContent.Password);
                return StatusCode(StatusCodes.Status200OK, new { Strength = strength});
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { Status = "Error while adding user data", e.Message });
            }
        }

        [HttpGet]
        public async Task<ActionResult<object>> GetUserData([FromQuery] string username)
        {
            try
            {
                var userdata = await _strongPasswordCheckService.GetUserData(username);
                return new ObjectResult(userdata);
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { Status = "Error while fetching user data", e.Message });
            }
        }

        [HttpPost]
        public async Task<ActionResult<object>> UpdateUserData([FromBody] UserData requestContent)
        {
            try
            {
                await _strongPasswordCheckService.UpdateUserData(requestContent.Username, requestContent.Password);
                return StatusCode(StatusCodes.Status200OK, new { Status = "Done"});
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { Status = "Error while updating user data", e.Message });
            }
        }

        [HttpDelete]
        public async Task<ActionResult<object>> DeleteUserData([FromQuery] string username)
        {
            try
            {
                await _strongPasswordCheckService.GetUserData(username);
                return StatusCode(StatusCodes.Status200OK, new { Status = "Done" });
            }
            catch(Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { Status = "Error while deleting user data", e.Message });
            }
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
