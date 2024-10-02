namespace ChessApi.Models.Messages
{
    public class InvalidMove
    {
        public string Message { get; set; }
        public InvalidMove(string piece, string error)
        {
            Message = $"Invalid {piece} movement: {error}";
        }
    }
}
