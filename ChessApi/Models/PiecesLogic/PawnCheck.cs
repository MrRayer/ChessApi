using System.Reflection.PortableExecutable;
using ChessApi.Models.Messages;
using ChessApi.Models.ResponseObjects;

namespace ChessApi.Models.PiecesLogic
{
    public static class PawnCheck
    {
        public static ResponseBS Check(Move move, List<List<string>> Board, bool IsWhite)
        {
            if (Board[move.Y2][move.X2] == "" && move.X1 == move.X2)
            {
                if ((move.Y1 - move.Y2 == 2 && IsWhite && move.Y1 == 6) || (move.Y2 - move.Y1 == 2 && !IsWhite && move.Y1 == 1))
                {
                    return new ResponseBS(true);
                }
                if ((move.Y1 - move.Y2 == 1 && IsWhite) || (move.Y2 - move.Y1 == 1 && !IsWhite))
                {
                    return new ResponseBS(true);
                }
                else
                {
                    return new ResponseBS(false, new InvalidMove("Pawn", "moved too much or in the wrong direction").Message);
                }
            }
            else if (Board[move.Y2][move.X2] != "" && Math.Abs(move.X1 - move.X2) == 1) //checks if it moves 1 to the side (doing this for mental mapping, it's hard to listen to a podcast while doing this)
            {
                if ((move.Y1 - move.Y2 == 1 && IsWhite) || (move.Y2 - move.Y1 == 1 && !IsWhite))
                {
                    return new ResponseBS(true);
                }
                else
                {
                    return new ResponseBS(false, new InvalidMove("Pawn", "moved too much or in the wrong direction").Message);
                }
            }
            else
            {
                return new ResponseBS(false, new InvalidMove("Pawn", "wrong pawn movement").Message);
            }
        }
    }
}
