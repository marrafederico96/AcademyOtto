using AuthLibrary.Data;
using AuthLibrary.Exceptions;
using AuthLibrary.Models;
using AuthLibrary.Repository;
using AuthLibrary.Security;

namespace AuthLibrary
{
    public class SqlService
    {
        private readonly SqlConnectionFactory _sqlConnectionProductionFactory;
        private readonly SqlConnectionFactory _sqlConnectionSecurityFactory;
        private readonly CustomerProductionRepository _customerProduction;
        private readonly CustomerSecurityRepository _customerSecurity;
        private readonly TokenSettings _tokenSettings;

        public SqlService(string connectionStringProduction, string connectionStringSecurity, TokenSettings tokenSettings)
        {
            _sqlConnectionProductionFactory = new SqlConnectionFactory(connectionStringProduction);
            _sqlConnectionSecurityFactory = new SqlConnectionFactory(connectionStringSecurity);
            _customerProduction = new CustomerProductionRepository(_sqlConnectionProductionFactory);
            _customerSecurity = new CustomerSecurityRepository(_sqlConnectionSecurityFactory);
            _tokenSettings = tokenSettings;
        }

        public async Task<string> LoginUser(UserLoginRequest userData, string role)
        {
            var userSecurity = await _customerSecurity.GetCustomerSecurityByEmailAsync(userData.EmailAddress)
                ?? throw new UnauthorizedAccessException("Wrong Credentials");

            if (userSecurity.ModifiedDate < DateTime.UtcNow.AddMonths(-6))
                throw new PasswordExpiredException("Password expired. Change it.");

            if (!PasswordService.CheckPassword(userData.Password, userSecurity.PasswordHash, userSecurity.PasswordSalt))
                throw new UnauthorizedAccessException("Wrong Credentials");

            return TokenService.GenerateJwtToken(userData.EmailAddress, role, _tokenSettings);
        }

        public async Task<bool> RegisterUser(UserRegisterRequest userData)
        {
            var customerID = await _customerProduction.InsertCustomerProductionAsync(userData);

            try
            {
                await _customerSecurity.InsertCustomerSecurityAsync(userData, customerID);
            }
            catch (Exception)
            {
                await _customerProduction.DeleteCustomerProductionAsync(customerID);
                throw new Exception();
            }

            return true;
        }

        public async Task<bool> RefreshPassword(string emailAddress, string newPassword)
        {
            var customer = await _customerSecurity.GetCustomerSecurityByEmailAsync(emailAddress)
                ?? throw new Exception("Customer not found");

            return await _customerSecurity.UpdatePasswordAsync(customer.CustomerID, newPassword);
        }

        public async Task<bool> DeleteCustomer(string emailAddress)
        {
            var customerSecurity = await _customerSecurity.GetCustomerSecurityByEmailAsync(emailAddress)
                ?? throw new Exception("Customer not found");

            bool deletedSec = await _customerSecurity.DeleteCustomerSecurityAsync(customerSecurity.CustomerID);

            if (!deletedSec)
            {
                throw new Exception();
            }

            try
            {
                bool deletedProd = await _customerProduction.DeleteCustomerProductionAsync(customerSecurity.CustomerID);
                return deletedSec && deletedProd;
            }
            catch (Exception)
            {
                await _customerSecurity.RollbackCustomerSecurity(customerSecurity);
                throw new Exception();
            }

        }
    }
}
