using AuthLibrary.Data;
using AuthLibrary.Exceptions;
using AuthLibrary.Models;
using AuthLibrary.Security;
using AuthLibrary.Services;
using Microsoft.Data.SqlClient;
using System.Data;

namespace AuthLibrary.Repository
{
    public class CustomerSecurityRepository(SqlConnectionFactory factory) : ICustomerSecurityRepository
    {
        public async Task<bool> InsertCustomerSecurityAsync(UserRegisterRequest userData, int newCustomerId)
        {
            using var connectionSec = await factory.CreateAsync();

            using var checkCmd = new SqlCommand(
                "SELECT 1 FROM Customer WHERE LOWER(TRIM(EmailAddress)) = @Email", connectionSec);
            checkCmd.Parameters.Add(UtilityService.CreateParam("@Email", SqlDbType.VarChar, userData.EmailAddress));

            var exists = await checkCmd.ExecuteScalarAsync();

            if (exists != null)
                throw new UserAlreadyExistsException($"User with email '{userData.EmailAddress}' already exists.");

            if (userData.Password != userData.ConfirmPassword)
                throw new PasswordMismatchException("Password mismatch.");

            var (Hash, Salt) = PasswordService.HashPassword(userData.Password);

            using var insertCmd = new SqlCommand(
                   @"INSERT INTO Customer 
                   (CustomerID, EmailAddress, PasswordHash, PasswordSalt, ModifiedDate, RowGuid)
                   VALUES (@CustomerID, @EmailAddress, @Hash, @Salt, @ModifiedDate, @RowGuid)",
                   connectionSec);

            insertCmd.Parameters.Add(UtilityService.CreateParam("@CustomerID", SqlDbType.Int, newCustomerId));
            insertCmd.Parameters.Add(UtilityService.CreateParam("@EmailAddress", SqlDbType.VarChar, userData.EmailAddress));
            insertCmd.Parameters.Add(UtilityService.CreateParam("@Hash", SqlDbType.VarChar, Hash, 256));
            insertCmd.Parameters.Add(UtilityService.CreateParam("@Salt", SqlDbType.VarChar, Salt, 64));
            insertCmd.Parameters.Add(UtilityService.CreateParam("@ModifiedDate", SqlDbType.DateTime, DateTime.UtcNow));
            insertCmd.Parameters.Add(UtilityService.CreateParam("@RowGuid", SqlDbType.UniqueIdentifier, Guid.NewGuid()));

            var result = await insertCmd.ExecuteNonQueryAsync();
            return result == 1;
        }

        public async Task<bool> UpdatePasswordAsync(int customerID, string newPassword)
        {
            using var connection = await factory.CreateAsync();

            var (Hash, Salt) = PasswordService.HashPassword(newPassword);

            using var cmd = new SqlCommand(@"UPDATE Customer SET
                                           PasswordHash = @Hash,
                                           PasswordSalt = @Salt,
                                           ModifiedDate = @ModifiedDate,
                                           WHERE CustomerID = @CustomerID",
                                           connection);

            cmd.Parameters.Add(UtilityService.CreateParam("@Hash", SqlDbType.VarChar, Hash, 256));
            cmd.Parameters.Add(UtilityService.CreateParam("@Salt", SqlDbType.VarChar, Salt, 64));
            cmd.Parameters.Add(UtilityService.CreateParam("@ModifiedDate", SqlDbType.DateTime, DateTime.UtcNow));
            cmd.Parameters.Add(UtilityService.CreateParam("@CustomerID", SqlDbType.Int, customerID));

            int result = await cmd.ExecuteNonQueryAsync();
            return result == 1;
        }

        public async Task<bool> DeleteCustomerSecurityAsync(int customerID)
        {
            using var connection = await factory.CreateAsync();
            using var cmd = new SqlCommand(@"DELETE FROM Customer 
                                           WHERE CustomerID = @CustomerID",
                                           connection);
            cmd.Parameters.Add(UtilityService.CreateParam("@CustomerID", SqlDbType.Int, customerID));
            int result = await cmd.ExecuteNonQueryAsync();
            return result == 1;
        }
        public async Task<UserSecurityData?> GetCustomerSecurityByEmailAsync(string email)
        {
            using var connection = await factory.CreateAsync();

            var cmd = new SqlCommand(
                "SELECT CustomerID, PasswordHash, PasswordSalt, ModifiedDate FROM Customer WHERE LOWER(TRIM(EmailAddress)) = @Email",
                connection);

            cmd.Parameters.Add(UtilityService.CreateParam("@Email", SqlDbType.VarChar, email));

            using var reader = await cmd.ExecuteReaderAsync();
            if (!await reader.ReadAsync())
                return null;

            return new UserSecurityData
            {
                CustomerID = reader.GetInt32(reader.GetOrdinal("CustomerID")),
                PasswordHash = reader.GetString(reader.GetOrdinal("PasswordHash")),
                PasswordSalt = reader.GetString(reader.GetOrdinal("PasswordSalt")),
                ModifiedDate = reader.GetDateTime(reader.GetOrdinal("ModifiedDate"))
            };
        }
    }
}