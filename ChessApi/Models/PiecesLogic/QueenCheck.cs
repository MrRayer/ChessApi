using ChessApi.Models.PiecesLogic.BasicLogic;
using ChessApi.Models.Messages;
using ChessApi.Models.ResponseObjects;

namespace ChessApi.Models.PiecesLogic
{
    public static class QueenCheck
    {
        public static ResponseBS Check(Move move, List<List<string>> Board, bool IsWhite)
        {
            if (move.X1 == move.X2 || move.Y1 == move.Y2 || Math.Abs(move.X1 - move.X2) == Math.Abs(move.Y1 - move.Y2))
            {
                if (!IsPathClear(move, Board))
                {
                    return new ResponseBS(false, new InvalidMove("Queen", "piece in the way").Message);
                }
                return new ResponseBS(true);
            }
            else
            {
                return new ResponseBS(false, new InvalidMove("Queen", "invalid queen movement").Message);
            }
        }
        private static bool IsPathClear(Move m, List<List<string>> Board)
        {
            Move _m = new Move(m.X1, m.Y1, m.X2, m.Y2);
            if (m.X1 == m.X2)                                             //Vertical
            {                                                            //
                while (Math.Abs(_m.Y1 - m.Y2) == 2)                     //if target is 2 steps away
                {                                                      //
                    _m.Y1 = Approach.HorVer(_m.Y1, m.Y2);             //approach vertically
                    if (_m.Y1 == -1) return false;                   //Error check
                    if (Board[_m.Y1][_m.X1] != "") return false;    //if not empty returns false
                }
                return true;
            }
            else if (m.Y1 == m.Y2)                                        //Horizontal
            {                                                            //
                while (Math.Abs(_m.X1 - m.X2) == 2)                     //if target is 2 steps away
                {                                                      //
                    _m.X1 = Approach.HorVer(_m.X1, m.X2);             //approach horizontally
                    if (_m.X1 == -1) return false;                   //Error check
                    if (Board[_m.Y1][_m.X1] != "") return false;    //if not empty returns false
                }
                return true;
            }
            else if (Math.Abs(m.X1 - m.X2) == Math.Abs(m.Y1 - m.Y2))                   //Diagonal
            {                                                                         //
                while (Math.Abs(_m.X1 - m.X2) >1 && Math.Abs(_m.Y1 - m.Y2) >1)   //if target is 2 steps away
                {                                                                   //
                    Tuple<int, int> ans = Approach.Diag(_m.X1, _m.Y1, m.X2, m.Y2); //approach diagonally
                    _m.X1 = ans.Item1;                                            //set X1
                    _m.Y1 = ans.Item2;                                           //set Y1
                    if (_m.X1 == -1 || _m.Y1 == -1) return false;               //Error check
                    if (Board[_m.Y1][_m.X1] != "") return false;               //if not empty returns false
                }
                    return true;
            }
            return false;
        }
    }
}
