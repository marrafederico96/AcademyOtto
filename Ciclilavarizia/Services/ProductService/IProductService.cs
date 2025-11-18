using Ciclilavarizia.Models.ProductModels.Dtos;

namespace Ciclilavarizia.Services.ProductService
{
    public interface IProductService
    {
        public Task<List<ProductResponse>> GetAllProductsAsync();

    }
}
