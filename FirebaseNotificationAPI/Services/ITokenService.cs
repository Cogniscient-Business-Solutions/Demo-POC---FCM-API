
using Microsoft.Data.SqlClient;
using System.Collections.Generic;
using System.Threading.Tasks;


namespace FirebaseNotificationAPI.Services
{
    public interface ITokenService
    {
        Task SaveTokenAsync(string userId, string token);
        Task DeleteTokenAsync(string userId);
        Task<List<string>> GetTokensByUserIdsAsync(List<string> userIds);
    }

    public class TokenService : ITokenService
    {
        private readonly SqlConnection _connection;

        public TokenService(SqlConnection connection)
        {
            _connection = connection;
        }

        public async Task SaveTokenAsync(string userId, string token)
        {
            using (var command = new SqlCommand("spSaveUserToken", _connection))
            {
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@UserId", userId);
                command.Parameters.AddWithValue("@Token", token);

                _connection.Open();
                await command.ExecuteNonQueryAsync();
                _connection.Close();
            }
        }

        public async Task DeleteTokenAsync(string userId)
        {
            using (var command = new SqlCommand("spDeleteUserToken", _connection))
            {
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@UserId", userId);

                _connection.Open();
                await command.ExecuteNonQueryAsync();
                _connection.Close();
            }
        }

        public async Task<List<string>> GetTokensByUserIdsAsync(List<string> userIds)
        {
            var tokens = new List<string>();

            using (var command = new SqlCommand("spGetTokensByUserIds", _connection))
            {
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@UserIds", string.Join(",", userIds));

                _connection.Open();
                using (var reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        tokens.Add(reader["Token"].ToString());
                    }
                }
                _connection.Close();
            }

            return tokens;
        }
    }
}
