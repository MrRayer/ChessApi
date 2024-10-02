namespace ChessApi.Models.ResponseObjects
{
    public interface IResponse
    {
        bool Status { get; set; }
        string Message { get; set; }
    }
}
