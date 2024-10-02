using ChessApi.Hubs;
using System.Collections.Concurrent;

namespace ChessApi.Services
{
    public class ConversationHubService : IConversationHubService
    {
        private readonly ConcurrentDictionary<string, bool> _activeConversations = new ConcurrentDictionary<string, bool>();

        public bool ConversationExists(string conversationId)
        {
            return _activeConversations.ContainsKey(conversationId);
        }

        public void AddConversation(string conversationId)
        {
            _activeConversations.TryAdd(conversationId, true);
        }
    }
}
