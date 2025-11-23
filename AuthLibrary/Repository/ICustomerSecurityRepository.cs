using AuthLibrary.Models;

namespace AuthLibrary.Repository
{
    public interface ICustomerSecurityRepository
    {
        public Task<bool> InsertCustomerSecurityAsync(UserRegisterRequest userData, int customerID);
        public Task<bool> UpdatePasswordAsync(int customerId, string newPassword);
        public Task<UserSecurityData?> GetCustomerSecurityByEmailAsync(string email);
        public Task<bool> DeleteCustomerSecurityAsync(int customerID);
        public Task RollbackCustomerSecurity(UserSecurityData userData);



    }
}
