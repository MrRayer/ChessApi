using ChessApi.ConstringHelpers;
using ChessApi.Models;
using ChessApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace ChessApi.Hubs
{
    [Authorize(Policy = "MustBeLogged")]
    public class PrivateMessagesHub : Hub
    {
        public async Task JoinConversation(string user2)
        {
            var user1 = Context.User.Identity.Name;
            string groupName = GetGroupName(user1, user2);
            await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
        }
        public async Task LeaveConversation(string user2)
        {
            var user1 = Context.User.Identity.Name;
            string groupName = GetGroupName(user1, user2);
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, groupName);
        }
        private string GetGroupName(string user1, string user2)
        {
            return string.Compare(user1, user2) < 0 ? $"{user1}_{user2}" : $"{user2}_{user1}";
        }
    }
}
