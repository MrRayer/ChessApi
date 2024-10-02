namespace ChessApi.Models.ResponseObjects
{
    public class ResponseBS : IResponse
    {
        public bool Status { get; set; }
        public string Message { get; set; }
        public ResponseBS(bool s,string m = "")
        {
            Status = s;
            Message = m;
        }
    }
}
