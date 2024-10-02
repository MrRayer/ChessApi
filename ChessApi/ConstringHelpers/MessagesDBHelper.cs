using ChessApi.Models.ResponseObjects;
using System.Data;
using Dapper;
using Microsoft.Data.SqlClient;
using ChessApi.Models;

namespace ChessApi.ConstringHelpers
{
    public class MessagesDBHelper
    {
        private string ConnString { get; set; }
        public MessagesDBHelper(IConfiguration config)
        {
            ConnString = config.GetValue<string>("ConnectionStrings:UserDB");
        }
        public ResponseBS SendMessage(string user1, string user2, string message, string time)
        {
            using (SqlConnection conn = new SqlConnection(ConnString))
            {
                var parameters = new DynamicParameters();
                parameters.Add("@_user1", user1);
                parameters.Add("@_user2", user2);
                parameters.Add("@_message", message);
                parameters.Add("@_time", time);
                var result = conn.QueryFirstOrDefault<string>("dbo.SendMessage", parameters, commandType: CommandType.StoredProcedure);
                if (result == "Ok") return new ResponseBS(true, "Success");
                else if (result != null) return new ResponseBS(false, result);
                else return new ResponseBS(false, "result empty from db");
            }
        }
        public ResponseBSML GetMessages(string user1, string user2)
        {
            using (SqlConnection conn = new SqlConnection(ConnString))
            {
                var parameters = new DynamicParameters();
                parameters.Add("@_user1", user1);
                parameters.Add("@_user2", user2);
                var result = conn.QueryMultiple("dbo.GetMessages", parameters, commandType: CommandType.StoredProcedure);
                var messages = new List<Message>();
                while (!result.IsConsumed)
                {
                    messages.AddRange(result.Read<Message>());
                }
                foreach (var message in messages)
                {
                    if (message.Username == null) return new ResponseBSML(false, "Username is null");
                    message.Username = message.Username.Trim();
                    message.Time = message.Time.Trim();
                }
                return new ResponseBSML(true, "Success", messages);
            }
        }
    }
}
