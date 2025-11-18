using AdventureWorks.Models.CustomerModel.Dtos;

namespace AdventureWorks.Services.CustomerService
{
    public interface ICustomerService
    {
        public Task<List<CustomerResponse>> GetCustomersAsync(int page, int pageSize);
    }
}
