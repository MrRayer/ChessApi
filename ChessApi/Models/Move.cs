using ChessApi.Models.ResponseObjects;

namespace ChessApi.Models
{
    public class Move
    {
        public int X1 { get; set; }
        public int Y1 { get; set; }
        public int X2 { get; set; }
        public int Y2 { get; set; }
        public Move() { }
        public Move(int _X1,int _Y1, int _X2, int _Y2)
        {
            X1 = _X1;
            Y1 = _Y1;
            X2 = _X2;
            Y2 = _Y2;
        }
        public ResponseBS Validate(List<List<string>> Board, bool IsWhite)
        {
            return MoveChecker.Check(this, IsWhite, Board);
        }
    }
}
