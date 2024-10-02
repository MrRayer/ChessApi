namespace ChessApi.ConstringHelpers
{
    public class MatchesDBHelper
    {
        private string ConnString { get; set; }
        public MatchesDBHelper(IConfiguration config)
        {
            ConnString = config.GetValue<string>("ConnectionString:MatchesDB");
        }
    }
}