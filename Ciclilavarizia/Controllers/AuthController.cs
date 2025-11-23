using AuthLibrary;
using AuthLibrary.Models;
using Ciclilavarizia.Exceptions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Ciclilavarizia.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class AuthController(SqlService sqlService) : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> Login([FromBody] UserLoginRequest userData)
        {
            var result = await sqlService.LoginUser(userData, "Admin");
            return Ok(new { token = result });
        }

        [HttpPost]
        public async Task<IActionResult> Register([FromBody] UserRegisterRequest userData)
        {
            var result = await sqlService.RegisterUser(userData);
            return Ok(new { registration = result });
        }

        [HttpPut]
        public async Task<ActionResult<bool>> RefreshPassword([FromBody] UserLoginRequest userData)
        {
            var result = await sqlService.RefreshPassword(userData.EmailAddress, userData.Password);
            return Ok(result);
        }

        [Authorize]
        [HttpDelete]
        public async Task<ActionResult<bool>> Delete([FromBody] UserDeleteRequest userData)
        {
            var result = await sqlService.DeleteCustomer(userData.EmailAddress);
            return Ok(result);
        }

        [Authorize]
        [HttpPut]
        public async Task<ActionResult<bool>> UpdateEmailAddress([FromBody] UserDeleteRequest userData)
        {
            var oldEmailAddress = User.Claims
                .FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value
                ?? throw new NotFoundException("Email not found");

            var result = await sqlService.UpdateCustomerEmail(oldEmailAddress, userData.EmailAddress);
            return Ok(result);
        }
    }
}