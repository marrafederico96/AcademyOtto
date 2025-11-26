using AdventureWorks.Models.ProductModels.Dtos;
using AdventureWorks.Services.ProductService;
using Microsoft.AspNetCore.Mvc;

namespace AdventureWorks.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class ProductController(IProductService productService) : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<List<ProductResponse>>> GetProducts(
            [FromQuery] int page = 1, [FromQuery] int pageSize = 10, [FromQuery] string category = "All")
        {
            var result = await productService.GetProductsAsync(page, pageSize, category);
            return Ok(result);
        }
    }
}
