using Azure.Core;
using ChessApi.Hubs;
using ChessApi.Models;
using ChessApi.Models.ResponseObjects;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using System.Collections.Concurrent;
using System.Security.Claims;

namespace ChessApi.Controllers
{
    [Route("api/ActiveMatches")]
    [ApiController]
    public class ActiveMatchesController : ControllerBase
    {
        private readonly ActiveMatchManager _activeMatches;
        private readonly IHubContext<MatchHub> _hubContext;
        public ActiveMatchesController(IHubContext<MatchHub> hubContext,ActiveMatchManager matchManager)
        {
            _hubContext = hubContext;
            _activeMatches = matchManager;
        }
        // ------ ENDPOINTS ------
        [Authorize(Policy = "MustBeLogged")]
        [HttpPost("CreateNewMatch")]
        public IActionResult CreateNewMatch()
        {
            _activeMatches.AddMatch(new Match());
            return Ok();
        }

        [HttpGet("GetActiveMatches")]
        public IActionResult GetActiveMatches()
        {
            List<MatchStringWithID> matchesInfo = new List<MatchStringWithID>();
            foreach (KeyValuePair<int, Match> kvp in _activeMatches.GetActiveMatches())
            {
                string matchInfo = $"id:{kvp.Key} | WP:{kvp.Value.WPlayer} | BP:{kvp.Value.BPlayer} | movements made:{kvp.Value.Moves.Count}";
                MatchStringWithID response = new(kvp.Key, matchInfo);
                matchesInfo.Add(response);
            }
            return Ok(matchesInfo);
        }

        [HttpGet("GetMatchById/{MatchId}")]
        public async Task<IActionResult> GetMatchById(int MatchId)
        {
            Match match = _activeMatches.GetMatchById(MatchId);
            if (match == null) return NotFound("id not found");
            return Ok(match);
        }

        [Authorize(Policy = "MustBeLogged")]
        [HttpPost("MakeMovement")]
        public async Task<IActionResult> MakeMovement(MovementRequest request)
        {
            Match match = _activeMatches.GetMatchById(request.MatchId);
            if (match == null) return BadRequest("Missing valid Match Id in move request");
            string userName = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value;
            if (userName == null) return BadRequest("Missing username in move request");
            ResponseBS MoveResponse = match.MakeMove(userName, request.move);
            if (!MoveResponse.Status) return BadRequest(MoveResponse.Message);
            await _hubContext.Clients.All.SendAsync("UpdateMatch", request.MatchId, _activeMatches.GetMatchById(request.MatchId));
            return Ok("Move successfull");
        }
        [Authorize(Policy = "MustBeLogged")]
        [HttpPost("SetWhites")]
        public async Task<IActionResult> SetWhites([FromBody] MatchIdRequest request)
        {
            if (request == null || request.MatchId <= 0) return BadRequest("Invalid MatchId");
            Match match = _activeMatches.GetMatchById(request.MatchId);
            if (match == null) return NotFound("Match not found");
            string userName = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value;
            if (string.IsNullOrEmpty(userName)) return Unauthorized("User not authenticated");
            if (!match.SetWhites(userName)) return BadRequest("Failed to set whites");
            await _hubContext.Clients.All.SendAsync("UpdateMatch", request.MatchId, _activeMatches.GetMatchById(request.MatchId));
            return Ok("Whites set successfully");
        }
        [Authorize(Policy = "MustBeLogged")]
        [HttpPost("SetBlacks")]
        public async Task<IActionResult> SetBlacks([FromBody] MatchIdRequest request)
        {
            if (request == null || request.MatchId <= 0) return BadRequest("Invalid MatchId");
            Match match = _activeMatches.GetMatchById(request.MatchId);
            if (match == null) return NotFound("Match not found");
            string userName = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value;
            if (string.IsNullOrEmpty(userName)) return Unauthorized("Username not found in claims");
            if (!match.SetBlacks(userName)) return BadRequest("Failed to set whites");
            await _hubContext.Clients.All.SendAsync("UpdateMatch", request.MatchId, _activeMatches.GetMatchById(request.MatchId));
            return Ok("Whites set successfully");
        }
    }
}
