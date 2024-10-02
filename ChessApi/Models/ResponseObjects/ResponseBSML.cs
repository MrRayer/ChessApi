namespace ChessApi.Models.ResponseObjects
{
    public class ResponseBSML : IResponse
    {
        public bool Status { get; set; }
        public string Message { get; set; }
        public List<Message> DBResponse { get; set; }
        public ResponseBSML(bool s, string m, List<Message> DBR = null)
        {
            Status = s;
            Message = m;
            DBResponse = DBR ?? new List<Message>();
        }
    }
}
