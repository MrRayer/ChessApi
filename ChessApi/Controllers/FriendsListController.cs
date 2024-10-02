using ChessApi.ConstringHelpers;
using ChessApi.Models.RequestObjects;
using ChessApi.Models.ResponseObjects;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ChessApi.Controllers
{
    [Authorize(Policy = "MustBeLogged")]
    [Route("api/[controller]")]
    [ApiController]
    public class FriendsListController : ControllerBase
    {
        private FriendsListDBHelper Helper;
        public FriendsListController(FriendsListDBHelper helper)
        {
            Helper = helper;
        }
        [HttpGet("GetFriendList")]
        public IActionResult GetFriendList()
        {
            string userName = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value;
            if (string.IsNullOrEmpty(userName)) return Unauthorized("User not authenticated");
            IEnumerable<string> response = Helper.GetFriendsList(userName);
            return Ok(response);
        }
        [HttpGet("GetPendingList")]
        public IActionResult GetPendingList()
        {
            string userName = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value;
            if (string.IsNullOrEmpty(userName)) return Unauthorized("User not authenticated");
            IEnumerable<string> response = Helper.GetPendingList(userName);
            return Ok(response);
        }
        [HttpPost("AddFriend")]
        public IActionResult AddFriend(JsonString target)
        {
            string user = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value;
            if (string.IsNullOrEmpty(user)) return Unauthorized("User not authenticated");
            ResponseBS response = Helper.AddFriend(user, target.Value);
            if (response.Status) return Ok(response.Message);
            else return BadRequest(response.Message);
        }
        [HttpPost("AcceptFriend")]
        public IActionResult AcceptFriend(JsonString target)
        {
            string user = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value;
            if (string.IsNullOrEmpty(user)) return Unauthorized("User not authenticated");
            ResponseBS response = Helper.AcceptFriend(user, target.Value);
            if (response.Status) return Ok(response.Message);
            else return BadRequest(response.Message);
        }
    }
}
