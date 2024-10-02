using ChessApi.ConstringHelpers;
using ChessApi.Hubs;
using ChessApi.Models;
using ChessApi.Models.MessagesEPModels;
using ChessApi.Models.ResponseObjects;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using System.Runtime.Intrinsics.X86;
using System.Security.Claims;

namespace ChessApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MessagesController : ControllerBase
    {
        private readonly IHubContext<PrivateMessagesHub> PMHub;
        private MessagesDBHelper Helper;
        public MessagesController(IHubContext<PrivateMessagesHub> _privateMessagesHub, MessagesDBHelper messagesDBHelper)
        {
            PMHub = _privateMessagesHub;
            Helper = messagesDBHelper;
        }
        [HttpPost("SendMessage")]
        [Authorize(Policy = "MustBeLogged")]
        public async Task<IActionResult> SendMessage(SendMessageProps props)
        {
            string userName = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value;
            if (string.IsNullOrEmpty(userName)) return Unauthorized("User not authenticated");
            DateTime now = DateTime.Now;
            string sentTime = $"{now.Hour}:{now.Minute}  {now.Day}/{now.Month}/{now.Year}";
            ResponseBS result = Helper.SendMessage(userName, props.user2, props.content, sentTime);
            if (result.Status == true)
            {
                string groupName = string.Compare(userName, props.user2) < 0 ? $"{userName}_{props.user2}" : $"{props.user2}_{userName}";
                await PMHub.Clients.Group(groupName).SendAsync("ReceiveMessages", Helper.GetMessages(userName, props.user2).DBResponse);
                return Ok(result.Message);
            }
            else return StatusCode(500, result.Message);
        }
        [HttpPost("GetMessages")]
        [Authorize(Policy = "MustBeLogged")]
        public IActionResult GetMessages(GetMessagesProps props)
        {
            string userName = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value;
            if (string.IsNullOrEmpty(userName)) return Unauthorized("User not authenticated");
            ResponseBSML result = Helper.GetMessages(userName,props.user2);
            if (result.Status == true) return Ok(result.DBResponse);
            else if (result.Message == "Username is null") return Ok();
            else return StatusCode(500, result.Message);
        }
    }
}
