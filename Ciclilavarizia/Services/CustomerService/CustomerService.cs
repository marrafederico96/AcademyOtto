using Ciclilavarizia.Data;
using Ciclilavarizia.Exceptions;
using Ciclilavarizia.Models.CustomerModel.Dtos;
using Microsoft.EntityFrameworkCore;

namespace Ciclilavarizia.Services.CustomerService
{
    public class CustomerService(CiclilavariziaContext context) : ICustomerService
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
                    Title = c.Title,
                    Phone = c.Phone,
                    SalesPerson = c.SalesPerson,
                    Suffix = c.Suffix,
                    CompanyName = c.CompanyName,
                }).ToListAsync();

            return customers;
        }

        public async Task<CustomerResponse> GetCustomerByIdAsync(int customerId)
        {
            var customer = await context.Customers
                .AsNoTracking()
                .Where(c => c.CustomerId == customerId)
                .Select(c => new CustomerResponse
                {
                    CustomerId = c.CustomerId,
                    FirstName = c.FirstName,
                    LastName = c.LastName,
                    MiddleName = c.MiddleName,
                    Title = c.Title,
                    Phone = c.Phone,
                    SalesPerson = c.SalesPerson,
                    Suffix = c.Suffix,
                    CompanyName = c.CompanyName,
                }).FirstOrDefaultAsync() ?? throw new NotFoundException($"Customer with ID {customerId} not found.");

            return customer;
        }
    }
}
