using AdventureWorks.Data;
using AdventureWorks.Models.CustomerModel.Dtos;
using Microsoft.EntityFrameworkCore;

namespace AdventureWorks.Services.CustomerService
{
    public class CustomerService(AdventureWorksLt2019Context context) : ICustomerService
    {
        public async Task<List<CustomerResponse>> GetCustomersAsync(int page, int pageSize)
        {
            var customers = await context.Customers
                .AsNoTracking()
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(c => new CustomerResponse
                {
                    CustomerId = c.CustomerId,
                    FirstName = c.FirstName,
                    LastName = c.LastName,
                    MiddleName = c.MiddleName,
                    EmailAddress = c.EmailAddress,
                    Title = c.Title,
                    Phone = c.Phone,
                    SalesPerson = c.SalesPerson,
                    Suffix = c.Suffix,
                    CompanyName = c.CompanyName,
                }).ToListAsync();

            return customers;
        }
    }
}
