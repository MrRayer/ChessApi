using ChessApi.Models.PiecesLogic.BasicLogic;
using ChessApi.Models.Messages;
using ChessApi.Models.ResponseObjects;

namespace ChessApi.Models.PiecesLogic
{
    public class BishopCheck
    {
        public static ResponseBS Check(Move move, List<List<string>> Board, bool IsWhite)
        {
            if (Math.Abs(move.X1 - move.X2)
                == Math.Abs(move.Y1 - move.Y2)) return new ResponseBS(false, new InvalidMove("Bishop", "not moved diagonally").Message);
            if (!IsPathClear(move, Board)) return new ResponseBS(false, new InvalidMove("Bishop", "piece in the way").Message);
            return new ResponseBS(true);
        }
        private static bool IsPathClear(Move m, List<List<string>> Board)
        {
            Move _m = new Move(m.X1, m.Y1, m.X2, m.Y2);
            while (_m.Y1 != _m.Y2 && _m.X1 != _m.X2)
            {
                Tuple<int, int> ans = Approach.Diag(_m.X1, _m.Y1, _m.X2, _m.Y2);
                if (ans.Item1 == -1 || ans.Item2 == -1) return false;
                _m.X1 = ans.Item1;
                _m.Y1 = ans.Item2;
                if (Board[_m.Y1][m.X1] != "") return false;
            }
            return true;
        }
    }
}
