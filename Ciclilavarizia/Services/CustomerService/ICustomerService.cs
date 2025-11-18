using Ciclilavarizia.Models.CustomerModel.Dtos;

namespace Ciclilavarizia.Services.CustomerService
{
    public interface ICustomerService
    {
        public Task<List<CustomerResponse>> GetCustomersAsync(int page, int pageSize);
        public Task<CustomerResponse> GetCustomerByIdAsync(int customerId);
    }
}
