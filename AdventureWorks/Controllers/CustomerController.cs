using AdventureWorks.Models.CustomerModel.Dtos;
using AdventureWorks.Services.CustomerService;
using Microsoft.AspNetCore.Mvc;

namespace AdventureWorks.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class CustomerController(ICustomerService customerService) : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<List<CustomerResponse>>> GetCustomers([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            try
            {
                var result = await customerService.GetCustomersAsync(page, pageSize);
                return Ok(new { customers = result });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }
    }
}
