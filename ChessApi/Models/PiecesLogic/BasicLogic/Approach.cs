namespace ChessApi.Models.PiecesLogic.BasicLogic
{
    public static class Approach
    {
        public static int HorVer(int N1, int N2)
        {
            if (N1 > N2) return N1 - 1;
            else if (N1 < N2) return N1 + 1;
            else return -1;
        }
        public static Tuple<int, int> Diag(int X1, int Y1, int X2, int Y2)
        {
            return new Tuple<int, int>(HorVer(X1, X2), HorVer(Y1, Y2));
        }
    }
}
