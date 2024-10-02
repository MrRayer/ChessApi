namespace ChessApi.Models.ResponseObjects
{
    public class MovementRequest
    {
        public int MatchId { get; set; }
        public Move move { get; set; }
    }
}
