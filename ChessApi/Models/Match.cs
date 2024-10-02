using ChessApi.Models.ResponseObjects;

namespace ChessApi.Models
{
    public class Match
    {
        public string WPlayer { get; private set; }
        public string BPlayer { get; private set; }
        public List<List<string>> Board { get; private set; }
        public List<string> Moves { get; private set; }
        public bool WhiteTurn { get; private set; }
        public bool IsActive { get; private set; }
        public string Winner { get; private set; }
        public Match()
        {
            WPlayer = "none";
            BPlayer = "none";
            Moves = new List<string>();
            WhiteTurn = true;
            IsActive = true;
            Winner = "none";
            Board = new List<List<string>>
            {
                new List<string> { "BR", "BKn", "BB", "BQ", "BKg", "BB", "BKn", "BR" },
                new List<string> { "BP", "BP", "BP", "BP", "BP", "BP", "BP", "BP" },
                new List<string> { "", "", "", "", "", "", "", "" },
                new List<string> { "", "", "", "", "", "", "", "" },
                new List<string> { "", "", "", "", "", "", "", "" },
                new List<string> { "", "", "", "", "", "", "", "" },
                new List<string> { "WP", "WP", "WP", "WP", "WP", "WP", "WP", "WP" },
                new List<string> { "WR", "WKn", "WB", "WQ", "WKg", "WB", "WKn", "WR" }
            };
        }
        public bool SetWhites(string name)
        {
            if (WPlayer != "none") return false;
            WPlayer = name;
            return true;
        }
        public bool SetBlacks(string name)
        {
            if (BPlayer != "none") return false;
            BPlayer = name;
            return true;
        }
        public ResponseBS MakeMove(string PName, Move move)
        {
            ResponseBS Checking = CheckMove(PName, move);
            if (!Checking.Status) return Checking;
            if (Board[move.Y1][move.X1][0] == "W"[0]) WhiteTurn = false;
            else WhiteTurn = true;
            Board[move.Y2][move.X2] = Board[move.Y1][move.X1];
            Board[move.Y1][move.X1] = "";
            return Checking;
        }
        private ResponseBS CheckMove(string PName, Move move)
        {
            bool IsWhite;
            if (WPlayer == PName) IsWhite = true;
            else if (BPlayer == PName) IsWhite = false;
            else return new ResponseBS(false, "User is not a player in this match");
            if (IsWhite && !WhiteTurn) return new ResponseBS(false,"White, it's black's turn");
            if (!IsWhite && WhiteTurn) return new ResponseBS(false,"Black, it's white's turn");
            return move.Validate(Board, IsWhite);
        }
        private void EndMatch(bool WhiteWins)
        {
            IsActive = false;
            if (WhiteWins) Winner = "Whites";
            if (!WhiteWins) Winner = "Blacks";
        }
    }
}
