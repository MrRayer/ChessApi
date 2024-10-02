using ChessApi.Hubs;

namespace ChessApi.Services
{
    public interface IConversationHubService
    {
        bool ConversationExists(string conversationId);
        void AddConversation(string conversationId);
    }
}
