using AuthLibrary.Models;

namespace AuthLibrary.Repository
{
    public interface ICustomerProductionRepository
    {
        public Task<int> InsertCustomerProductionAsync(UserRegisterRequest userData);
        public Task<bool> DeleteCustomerProductionAsync(int customerID);

    }
}
