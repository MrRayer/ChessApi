using ChessApi.Models.PiecesLogic;
using ChessApi.Models.ResponseObjects;

namespace ChessApi.Models
{
    public static class MoveChecker
    {
        public static ResponseBS Check(Move move, bool IsWhite, List<List<string>> Board)
        {
            if (Board[move.Y1][move.X1][0] == 'W' && !IsWhite)
                return new ResponseBS(false, "Black player moving white piece");
            if (Board[move.Y1][move.X1][0] == 'B' && IsWhite)
                return new ResponseBS(false, "White player moving black piece");
            if (move.X1 == move.X2 && move.Y1 == move.Y2)
                return new ResponseBS(false, "No movement made");
            if (Board[move.Y2][move.X2] != "")
            {
                if (Board[move.Y1][move.X1][0] == Board[move.Y2][move.X2][0])
                    return new ResponseBS(false, "Friendly fire");
            }

            switch (Board[move.Y1][move.X1].Substring(1))
            {
                case "P":
                    return PawnCheck.Check(move, Board, IsWhite);
                case "R":
                    return RookCheck.Check(move, Board, IsWhite);
                case "Kn":
                    return KnightCheck.Check(move, Board, IsWhite);
                case "B":
                    return BishopCheck.Check(move, Board, IsWhite);
                case "Q":
                    return QueenCheck.Check(move, Board, IsWhite);
                case "Kg":
                    return KingCheck.Check(move, Board, IsWhite);
                default:
                    return new ResponseBS(false, "Unknown piece type");
            }
        }
    }
}
