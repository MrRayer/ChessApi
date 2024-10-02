using ChessApi.Hubs;
using ChessApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using System.Security.Claims;

namespace ChessApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChatController : ControllerBase
    {
        private readonly MainChatManager _mainChat;
        private readonly IHubContext<MessageHub> _messageHub;
        public ChatController(IHubContext<MessageHub> MH,MainChatManager MCM)
        {
            _messageHub = MH;
            _mainChat = MCM;
        }
        [Authorize(Policy = "MustBeLogged")]
        [HttpPost("AddMessage")]
        public async Task<IActionResult> AddMessage([FromBody] Message message)
        {
            if (message == null) return BadRequest("Message is null");
            try
            {
                string userName = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value;
                if (string.IsNullOrEmpty(userName)) return Unauthorized("User not authenticated");
                message.Username = userName;
                DateTime now = DateTime.Now;
                message.Time = $"{now.Hour}:{now.Minute}  {now.Day}/{now.Month}/{now.Year}";
                await Task.Run(() => _mainChat.AddMessage(message));
                await _messageHub.Clients.All.SendAsync("UpdateChat", await Task.Run(() => _mainChat.GetMessages()));
                return Ok("Message added");
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }
        [HttpGet("GetMessages")]
        public async Task<IActionResult> GetMessages()
        {
            try
            {
                IEnumerable<KeyValuePair<int, Message>> messages = await Task.Run(() => _mainChat.GetMessages());
                return Ok(messages);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }
    }
}
