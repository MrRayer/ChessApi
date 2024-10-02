using System.Collections.Concurrent;

namespace ChessApi.Models
{
    public class MainChatManager
    {
        private readonly ConcurrentDictionary<int, Message> _chat = new ConcurrentDictionary<int, Message>();
        private readonly Queue<int> _messageOrder = new Queue<int>();
        private int _nextId = 1;
        private readonly int _maxMessages = 30;

        public void AddMessage(Message message)
        {
            int id = _nextId++;
            _chat.TryAdd(id, message);
            _messageOrder.Enqueue(id);
            ClearOldChat();
        }
        private void ClearOldChat()
        {
            while (_messageOrder.Count > _maxMessages)
            {
                int oldestId = _messageOrder.Dequeue();
                _chat.TryRemove(oldestId, out _);
            }
        }
        public IEnumerable<KeyValuePair<int, Message>> GetMessages()
        {
            return _chat.OrderBy(kv => kv.Key).Select(kv => new KeyValuePair<int, Message>(kv.Key, kv.Value));
        }
    }
}
