using ChessApi.Models.Messages;
using ChessApi.Models.ResponseObjects;

namespace ChessApi.Models.PiecesLogic
{
    public static class KnightCheck
    {
        public static ResponseBS Check(Move move, List<List<string>> Board, bool IsWhite)
        {
            if (Math.Abs(move.X1 - move.X2) == 2 && Math.Abs(move.Y1 - move.Y2) == 1
                || Math.Abs(move.X1 - move.X2) == 1 && Math.Abs(move.Y1 - move.Y2) == 2)
            {
                return new ResponseBS(true);
            }
            else
            {
                return new ResponseBS(false, new InvalidMove("Knight", "wrong knightly movement").Message);
            }
        }
    }
}
