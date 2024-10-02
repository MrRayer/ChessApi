using ChessApi.Models;
using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace ChessApi.Hubs
{
    public class MatchHub : Hub
    {
        public async Task UpdateMatch(int id, Match match)
        {
            await Clients.All.SendAsync("UpdateMatch", id, match);
        }
    }
}