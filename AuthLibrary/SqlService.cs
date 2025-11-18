using AuthLibrary.Models;
using Microsoft.Data.SqlClient;

namespace AuthLibrary
{
    public class SqlService(string connectionString, TokenSettings tokenSettings)
    {
        public async Task<string> LoginUser(UserLoginRequest userData, string role)
        {
            using var connection = new SqlConnection(connectionString);
            await connection.OpenAsync();

            string query = "SELECT PasswordHash, PasswordSalt FROM Customers WHERE LOWER(TRIM(EmailAddress)) = @Email";
            var command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@Email", userData.Email.Trim().ToLowerInvariant());

            var reader = await command.ExecuteReaderAsync();

            string passwordHash = string.Empty;
            string passwordSalt = string.Empty;

            if (!reader.HasRows)
            {
                throw new UnauthorizedAccessException("Wrong credentials");
            }
            else
            {
                while (reader.Read())
                {
                    passwordHash = reader.GetString(reader.GetOrdinal("PasswordHash"));
                    passwordSalt = reader.GetString(reader.GetOrdinal("PasswordSalt"));
                }
            }

            PasswordService.HashPassword(userData.Password, passwordHash, passwordSalt);

            var token = TokenService.GenerateJwtToken(userData.Email, role, tokenSettings);
            return token;
        }
    }
}
