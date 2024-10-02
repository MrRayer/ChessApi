using ChessApi.Models;
using System.Data;
using Dapper;
using Microsoft.Data.SqlClient;

namespace ChessApi.ConstringHelpers
{
    public class UserDBHelper
    {
        private string ConnString { get; set; }
        public UserDBHelper(IConfiguration config)
        {
            ConnString = config.GetValue<string>("ConnectionStrings:UserDB");
        }
        public string Validate(Users user)
        {
            string role = "";
            using (SqlConnection conn = new SqlConnection(ConnString))
            {
                var parameters = new DynamicParameters();
                parameters.Add("@Name", user.Name);
                parameters.Add("@Password", user.Password);
                parameters.Add("@Role", dbType: DbType.String, direction: ParameterDirection.Output, size: 20);

                conn.Execute("dbo.ValidateUser", parameters, commandType: CommandType.StoredProcedure);

                // Get the value of the output parameter Role
                role = parameters.Get<string>("@Role");
            }
            return role;
        }
        public bool CreateUser(Users user)
        {
            bool isSuccess = false;
            using (SqlConnection conn = new SqlConnection(ConnString))
            {
                var parameters = new DynamicParameters();
                parameters.Add("@Name", user.Name);
                parameters.Add("@Password", user.Password);
                parameters.Add("@Email", user.Email);
                parameters.Add("@Success", dbType: DbType.Boolean, direction: ParameterDirection.Output);

                conn.Execute("dbo.CreateUser", parameters, commandType: CommandType.StoredProcedure);

                // Get the value of the output parameter Success
                isSuccess = parameters.Get<bool>("@Success");
            }
            return isSuccess;
        }
    }
}