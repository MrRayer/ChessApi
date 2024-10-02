using ChessApi.ConstringHelpers;
using ChessApi.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ChessApi.Controllers
{
    [Route("api/User")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private UserDBHelper Helper;
        public UserController(UserDBHelper helper)
        {
            Helper = helper;
        }
        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] Users user)
        {
            if (user == null) return BadRequest("empty user or not following Users model");
            string role = Helper.Validate(user);
            if (role == "Invalid") return StatusCode(401, "User is not validated");
            if (role == "NotFound") return NotFound("User not found");
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Name),
                new Claim(ClaimTypes.Role, role),
            };
            var identity = new ClaimsIdentity(claims, "UserAuthentication");
            ClaimsPrincipal principal = new(identity);
            await HttpContext.SignInAsync("UserAuthentication", principal);
            return Ok("Success");
        }
        [HttpGet("Logout")]
        [Authorize(Policy = "MustBeLogged")]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync("UserAuthentication");
            return Ok("Logged out successfully");
        }
        [HttpPost("CreateUser")]
        public IActionResult CreateUser(Users user)
        {
            if (user == null) return BadRequest("empty user or not not following Users model");
            if (!Helper.CreateUser(user)) return BadRequest("Fail to add user, user already on db?");
            return Ok("Success");
        }
        [HttpPost("UpdateUser")]
        [Authorize(Policy = "MustBeLogged")]
        public IActionResult UpdateUser()
        {
            return Ok("Success");
        }
        [HttpPost("DeleteUser")]
        [Authorize(Policy = "MustBeLogged")]
        public IActionResult DeleteUser()
        {
            return Ok("Success");
        }
        [HttpGet("error403")]
        public IActionResult error403()
        {
            return StatusCode(401, "Unauthorized");
        }
        [HttpGet("error401")]
        public IActionResult error401()
        {
            return StatusCode(401, "must log in");
        }
    }
}
