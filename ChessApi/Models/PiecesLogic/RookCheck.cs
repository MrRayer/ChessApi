using ChessApi.Models.Messages;
using ChessApi.Models.PiecesLogic.BasicLogic;
using ChessApi.Models.ResponseObjects;

namespace ChessApi.Models.PiecesLogic
{
    public static class RookCheck
    {
        public static ResponseBS Check(Move move, List<List<string>> Board, bool IsWhite)
        {
            if (move.X1 == move.X2 || move.Y1 == move.Y2)
            {
                if (!IsPathClear(move, Board))
                {
                    return new ResponseBS(false, "Piece in the way");
                }
                return new ResponseBS(true);
            }
            else
            {
                return new ResponseBS(false, new InvalidMove("Rook", "not moving vertically/diagonally").Message);
            }
        }
        private static bool IsPathClear(Move m, List<List<string>> Board)
        {
            Move _m = new Move(m.X1, m.Y1, m.X2, m.Y2);
            if (m.X1 == m.X2)                                        //Vertical move
            {                                                       //
                while (Math.Abs(_m.Y1 - m.Y2) == 2)                //if target is 2 steps away
                {                                                 //
                    _m.Y1 = Approach.HorVer(_m.Y1, _m.Y2);       //Aproach Vertically
                    if (_m.Y1 == -1) return false;              //error catch
                    if (Board[_m.Y1][m.X1] != "") return false;//if not empty returns false
                }
                return true;
            }
            else if (m.Y1 == m.Y2)                                   //Horizontal move
            {                                                       //
                while (Math.Abs(_m.X1 - m.X2) == 2)                //if target is 2 steps away
                {                                                 //
                    _m.X1 = Approach.HorVer(_m.X1, _m.X2);       //Aproach Horizontally
                    if (_m.X1 == -1) return false;              //error catch
                    if (Board[m.Y1][_m.X1] != "") return false;//if not empty returns false
                }
                return true;
            }
            else return false;
        }        
    }
}
