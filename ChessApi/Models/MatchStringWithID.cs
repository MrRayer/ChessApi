namespace ChessApi.Models
{
    public class MatchStringWithID
    {
        public int Id { get; set; }
        public string String { get; set; }
        public MatchStringWithID(int _id, string _string)
        {
            Id = _id;
            String = _string;
        }
    }
}
