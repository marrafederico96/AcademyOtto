using Ciclilavarizia.Models.ProductModels.Dtos;
using Ciclilavarizia.Services.ProductService;
using Microsoft.AspNetCore.Mvc;

namespace Ciclilavarizia.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class ProductController(IProductService productService) : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<List<ProductResponse>>> GetProducts(
            [FromQuery] int page = 1, [FromQuery] int pageSize = 10, [FromQuery] string categoryName = "All")
        {
            var result = await productService.GetProductsAsync(page, pageSize, categoryName);
            return Ok(result);
        }

    }
}
