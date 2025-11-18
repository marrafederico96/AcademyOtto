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
        public async Task<ActionResult<List<ProductResponse>>> GetCollections()
        {
            var result = await productService.GetAllProductsAsync();
            return Ok(result);
        }
    }
}
