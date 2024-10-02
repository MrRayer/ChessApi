using System.Collections.Concurrent;

namespace ChessApi.Models
{
    public class ActiveMatchManager
    {
        private readonly ConcurrentDictionary<int, Match> _activeMatches = new ConcurrentDictionary<int, Match>();

        public void AddMatch(Match match)
        {
            int id;
            if (_activeMatches.Count == 0) id = 1;
            else id = _activeMatches.Keys.Max() + 1;
            _activeMatches.TryAdd(id, match);
        }

        public bool RemoveMatch(int id)
        {
            return _activeMatches.TryRemove(id, out _);
        }

        public IEnumerable<KeyValuePair<int, Match>> GetActiveMatches()
        {
            return _activeMatches.Select(kv => new KeyValuePair<int, Match>(kv.Key, kv.Value));
        }

        public Match GetMatchById(int id)
        {
            _activeMatches.TryGetValue(id, out Match match);
            return match;
        }
    }
}
