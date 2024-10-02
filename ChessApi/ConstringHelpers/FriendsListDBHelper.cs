using ChessApi.Models.ResponseObjects;
using Dapper;
using Microsoft.Data.SqlClient;
using System.Data;

namespace ChessApi.ConstringHelpers
{
    public class FriendsListDBHelper
    {
        private string ConnString { get; set; }
        public FriendsListDBHelper(IConfiguration config)
        {
            ConnString = config.GetValue<string>("ConnectionStrings:UserDB");
        }
        public IEnumerable<string> GetFriendsList(string user)
        {
            using (SqlConnection conn = new SqlConnection(ConnString))
            {
                var parameters = new DynamicParameters();
                parameters.Add("@User", user);
                var result = conn.Query<string>("dbo.GetFriends", parameters, commandType: CommandType.StoredProcedure);
                if (result != null) { return  result; }
                else return new List<string>();
            }
        }
        public IEnumerable<string> GetPendingList(string user)
        {
            using (SqlConnection conn = new SqlConnection(ConnString))
            {
                var parameters = new DynamicParameters();
                parameters.Add("@User", user);
                var result = conn.Query<string>("dbo.GetPending", parameters, commandType: CommandType.StoredProcedure);
                if (result != null) { return result; }
                else return new List<string>();
            }
        }
        public ResponseBS AddFriend(string user, string target)
        {
            using (SqlConnection conn = new SqlConnection(ConnString))
            {
                var parameters = new DynamicParameters();
                parameters.Add("@User1", user);
                parameters.Add("@User2", target);
                parameters.Add("@ResultMessage", dbType: DbType.String, direction: ParameterDirection.Output, size: 50);
                conn.Execute("AddFriend", parameters, commandType: CommandType.StoredProcedure);
                string resultMessage = parameters.Get<string>("@ResultMessage");
                if (resultMessage == "Success") { return new ResponseBS(true, resultMessage); }
                else if (resultMessage == "Fail") { return new ResponseBS(false, "Undocumented failure"); }
                else return new ResponseBS(false, resultMessage);
            }
        }
        public ResponseBS AcceptFriend(string user, string target)
        {
            using (SqlConnection conn = new SqlConnection(ConnString))
            {
                var parameters = new DynamicParameters();
                parameters.Add("@user", user);
                parameters.Add("@target", target);
                parameters.Add("@statusMessage", dbType: DbType.String, direction: ParameterDirection.Output, size: 50);
                conn.Execute("AcceptFriend", parameters, commandType: CommandType.StoredProcedure);
                string statusMessage = parameters.Get<string>("@statusMessage");
                if (statusMessage == "Success") { return new ResponseBS(true, statusMessage); }
                else if (statusMessage == "Fail") { return new ResponseBS(false, "Undocumented failure"); }
                else return new ResponseBS(false, statusMessage);
            }
        }
    }
}
