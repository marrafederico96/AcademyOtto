using AdventureWorks.Models.CustomerModel.Dtos;
using AdventureWorks.Services.CustomerService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AdventureWorks.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class CustomerController(ICustomerService customerService) : ControllerBase
    {
        [Authorize]
        [HttpGet]
        public async Task<ActionResult<List<CustomerResponse>>> GetCustomers([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            var result = await customerService.GetCustomersAsync(page, pageSize);
            return Ok(new { customers = result });
        }

        [Authorize]
        [HttpGet("{customerId}")]
        public async Task<ActionResult<CustomerResponse>> GetCustomerById(int customerId)
        {
            var result = await customerService.GetCustomerByIdAsync(customerId);
            return Ok(new { customer = result });
        }

    }
}
