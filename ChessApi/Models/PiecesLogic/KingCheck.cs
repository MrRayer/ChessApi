using ChessApi.Models.Messages;
using ChessApi.Models.ResponseObjects;

namespace ChessApi.Models.PiecesLogic
{
    public static class KingCheck
    {
        public static ResponseBS Check(Move move, List<List<string>> board, bool IsWhite)
        {
            if (Math.Abs(move.X1 - move.X2) > 1)
            {
                return new ResponseBS(false, new InvalidMove("King", "movement bigger than 1").Message);
            }
            if (Math.Abs(move.Y1 - move.Y2) > 1)
            {
                return new ResponseBS(false, new InvalidMove("King", "movement bigger than 1").Message);
            }
            return new ResponseBS(true);
        }
    }
}
