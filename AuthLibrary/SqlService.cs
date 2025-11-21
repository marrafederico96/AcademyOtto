using AuthLibrary.Exceptions;
using AuthLibrary.Models;
using Microsoft.Data.SqlClient;
using System.Data;

namespace AuthLibrary
{
    public class SqlService(string connectionStringProd, string connectionStringSec, TokenSettings tokenSettings)
    {
        public async Task<string> LoginUser(UserLoginRequest userData, string role)
        {
            using var connection = new SqlConnection(connectionStringSec);
            await connection.OpenAsync();

            string query = "SELECT PasswordHash, PasswordSalt, ModifiedDate FROM Customer WHERE LOWER(TRIM(EmailAddress)) = @Email";
            var command = new SqlCommand(query, connection);
            var param = new SqlParameter("@Email", SqlDbType.NVarChar, 256)
            {
                Value = userData.EmailAddress.Trim().ToLowerInvariant()
            };
            command.Parameters.Add(param);

            using var reader = await command.ExecuteReaderAsync();

            if (!reader.Read())
            {
                throw new UnauthorizedAccessException("Wrong Credentials");
            }

            string passwordHash = reader.GetString(reader.GetOrdinal("PasswordHash"));
            string passwordSalt = reader.GetString(reader.GetOrdinal("PasswordSalt"));
            DateTime modifiedDate = reader.GetDateTime(reader.GetOrdinal("ModifiedDate"));

            if (modifiedDate < DateTime.UtcNow.AddMonths(-6))
            {
                throw new PasswordExpiredException("Password expired. Change it.");
            }

            if (!PasswordService.CheckPassword(userData.Password, passwordHash, passwordSalt))
            {
                throw new UnauthorizedAccessException("Wrong Credentials");
            }



            var token = TokenService.GenerateJwtToken(userData.EmailAddress, role, tokenSettings);
            return token;
        }

        public async Task<bool> RegisterUser(UserRegisterRequest userData)
        {
            using var connectionSecurity = new SqlConnection(connectionStringSec);
            await connectionSecurity.OpenAsync();

            string query = "SELECT 1 FROM Customer WHERE LOWER(TRIM(EmailAddress)) = @EmailAddress";
            var verifyEmail = new SqlCommand(query, connectionSecurity);

            var emailParam = CreateParam("@EmailAddress", SqlDbType.VarChar, userData.EmailAddress);
            verifyEmail.Parameters.Add(emailParam);

            using var reader = await verifyEmail.ExecuteReaderAsync();

            if (await reader.ReadAsync())
            {
                throw new UserAlreadyExistsException($"Registration Failed. User with email '{userData.EmailAddress}' already exists.");
            }
            await reader.CloseAsync();

            if (userData.Password != userData.ConfirmPassword)
            {
                throw new PasswordMismatchException("Password not match");
            }

            await connectionSecurity.CloseAsync();
            using var connectionProd = new SqlConnection(connectionStringProd);
            await connectionProd.OpenAsync();

            var parameters = new[]
            {
                CreateParam("@FirstName", SqlDbType.VarChar, userData.FirstName),
                CreateParam("@LastName", SqlDbType.VarChar, userData.LastName),
                CreateParam("@MiddleName", SqlDbType.VarChar, userData.MiddleName),
                CreateParam("@Title", SqlDbType.VarChar, userData.Title),
                CreateParam("@Suffix", SqlDbType.VarChar, userData.Suffix),
                CreateParam("@CompanyName", SqlDbType.VarChar, userData.CompanyName),
                CreateParam("@SalesPerson", SqlDbType.VarChar, userData.SalesPerson),
                CreateParam("@Phone", SqlDbType.VarChar, userData.Phone),
                CreateParam("@ModifiedDate",SqlDbType.DateTime, DateTime.UtcNow),
                CreateParam("@RowGuid", SqlDbType.UniqueIdentifier, Guid.NewGuid())
            };

            var insertQueryProd = "INSERT INTO SalesLT.Customer (FirstName,LastName,MiddleName,Title,CompanyName,SalesPerson,Phone,Suffix,ModifiedDate,Rowguid)" +
                "VALUES (@FirstName,@LastName,@MiddleName,@Title,@CompanyName,@SalesPerson,@Phone,@Suffix,@ModifiedDate,@RowGuid)";

            var registerProdCommand = new SqlCommand(insertQueryProd, connectionProd);

            foreach (var param in parameters)
            {
                registerProdCommand.Parameters.Add(param);
            }

            var resultProd = await registerProdCommand.ExecuteNonQueryAsync();
            connectionProd.Close();

            var resultSec = await RegisterSecurity(connectionStringSec, userData.Password, userData.EmailAddress);

            if (resultProd == resultSec && resultProd == 1 && resultSec == 1)
            {
                return true;
            }
            else
            {
                return false;
            }

        }

        private async static Task<int> RegisterSecurity(string securityString, string password, string email)
        {
            using var connectionSecurity = new SqlConnection(securityString);
            await connectionSecurity.OpenAsync();

            var (Hash, Salt) = PasswordService.HashPassword(password);

            var hashParam = CreateParam("@PasswordHash", SqlDbType.VarChar, Hash, 256);
            var saltParam = CreateParam("@PasswordSalt", SqlDbType.VarChar, Salt, 32);
            var emailAddressParam = CreateParam("@EmailAddress", SqlDbType.VarChar, email, 50);
            var modifiedDateParam = CreateParam("@ModifiedDate", SqlDbType.DateTime, DateTime.UtcNow);
            var rowGuidParam = CreateParam("@RowGuid", SqlDbType.UniqueIdentifier, Guid.NewGuid());

            var insertQuery = "INSERT INTO Customer (EmailAddress,PasswordHash,PasswordSalt,ModifiedDate,Rowguid) " +
                "VALUES (@EmailAddress,@PasswordHash,@PasswordSalt,@ModifiedDate,@RowGuid)";

            var registerSecurity = new SqlCommand(insertQuery, connectionSecurity);

            registerSecurity.Parameters.Add(hashParam);
            registerSecurity.Parameters.Add(saltParam);
            registerSecurity.Parameters.Add(emailAddressParam);
            registerSecurity.Parameters.Add(modifiedDateParam);
            registerSecurity.Parameters.Add(rowGuidParam);

            var result = await registerSecurity.ExecuteNonQueryAsync();
            return result;
        }

        private static SqlParameter CreateParam(string name, SqlDbType type, object? value, int size = 50)
        {
            object sqlValue = DBNull.Value;

            if (value != null)
            {
                if (value is string strValue)
                {
                    if (!string.IsNullOrWhiteSpace(strValue))
                    {
                        sqlValue = strValue.TrimStart().TrimEnd();
                    }
                }
                else
                {
                    sqlValue = value;
                }
            }

            return new SqlParameter(name, type, size)
            {
                Value = sqlValue
            };
        }

    }
}
