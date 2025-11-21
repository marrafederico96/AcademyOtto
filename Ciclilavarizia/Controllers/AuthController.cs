using AuthLibrary;
using AuthLibrary.Models;
using Microsoft.AspNetCore.Mvc;

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
            return Ok();
        }
    }
}