using AuthLibrary.Data;
using AuthLibrary.Models;
using AuthLibrary.Services;
using Microsoft.Data.SqlClient;
using System.Data;

namespace AuthLibrary.Repository
{
    public class CustomerProductionRepository(SqlConnectionFactory factory) : ICustomerProductionRepository
    {

        public async Task<int> InsertCustomerProductionAsync(UserRegisterRequest userData)
        {
            using var connectionProd = await factory.CreateAsync();

            string insertProd = @"INSERT INTO SalesLT.Customer (FirstName, LastName, MiddleName, Title, CompanyName, 
                                SalesPerson, Phone, Suffix, ModifiedDate, Rowguid)
                                OUTPUT INSERTED.CustomerID
                                VALUES (@FirstName, @LastName, @MiddleName, @Title, @CompanyName,
                                @SalesPerson, @Phone, @Suffix, @ModifiedDate, @RowGuid)";

            using var cmdProd = new SqlCommand(insertProd, connectionProd);

            cmdProd.Parameters.AddRange(
            [
                UtilityService.CreateParam("@FirstName", SqlDbType.VarChar, userData.FirstName),
                UtilityService.CreateParam("@LastName", SqlDbType.VarChar, userData.LastName),
                UtilityService.CreateParam("@MiddleName", SqlDbType.VarChar, userData.MiddleName),
                UtilityService.CreateParam("@Title", SqlDbType.VarChar, userData.Title),
                UtilityService.CreateParam("@CompanyName", SqlDbType.VarChar, userData.CompanyName),
                UtilityService.CreateParam("@SalesPerson", SqlDbType.VarChar, userData.SalesPerson),
                UtilityService.CreateParam("@Phone", SqlDbType.VarChar, userData.Phone),
                UtilityService.CreateParam("@Suffix", SqlDbType.VarChar, userData.Suffix),
                UtilityService.CreateParam("@ModifiedDate", SqlDbType.DateTime, DateTime.UtcNow),
                UtilityService.CreateParam("@RowGuid", SqlDbType.UniqueIdentifier, Guid.NewGuid())
            ]);

            int newCustomerId = (int)await cmdProd.ExecuteScalarAsync();
            return newCustomerId;
        }

        public async Task<bool> DeleteCustomerProductionAsync(int customerID)
        {
            using var connection = await factory.CreateAsync();
            using var cmd = new SqlCommand(@"DELETE FROM SalesLT.Customer 
                                           WHERE CustomerID = @CustomerID",
                                           connection);

            cmd.Parameters.Add(UtilityService.CreateParam("@CustomerID", SqlDbType.Int, customerID));

            int result = await cmd.ExecuteNonQueryAsync();
            return result == 1;
        }

        public async Task<UserProductionData?> GetCustomerProductionByIdAsync(int customerID)
        {
            using var connection = await factory.CreateAsync();

            var cmd = new SqlCommand(
                "SELECT CustomerID, EmailAddress, Title, FirstName, MiddleName, LastName, ModifiedDate, Suffix, CompanyName, SalesPerson, Phone " +
                "FROM Customer WHERE customerID = @CustomerID",
                connection);

            cmd.Parameters.Add(UtilityService.CreateParam("@CustomerID", SqlDbType.Int, customerID));

            using var reader = await cmd.ExecuteReaderAsync();
            if (!await reader.ReadAsync())
                return null;

            return new UserProductionData
            {
                CustomerID = reader.GetInt32(reader.GetOrdinal("CustomerID")),
                EmailAddress = reader.GetString(reader.GetOrdinal("EmailAddress")),
                CompanyName = reader.GetString(reader.GetOrdinal("ComapnyName")),
                FirstName = reader.GetString(reader.GetOrdinal("FirstName")),
                LastName = reader.GetString(reader.GetOrdinal("LastName")),
                MiddleName = reader.GetString(reader.GetOrdinal("MiddleName")),
                Phone = reader.GetString(reader.GetOrdinal("Phone")),
                SalesPerson = reader.GetString(reader.GetOrdinal("SalesPerson")),
                Suffix = reader.GetString(reader.GetOrdinal("Suffix")),
                Title = reader.GetString(reader.GetOrdinal("Title"))
            };
        }

    }
}
