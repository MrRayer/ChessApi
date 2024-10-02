using ChessApi.Models;
using Microsoft.AspNetCore.SignalR;

namespace ChessApi.Hubs
{
    public class MessageHub : Hub
    {
        public async Task UpdateChat(IEnumerable<KeyValuePair<int, Message>> chat)
        {
            await Clients.All.SendAsync("UpdateChat", chat);
        }
    }
}
