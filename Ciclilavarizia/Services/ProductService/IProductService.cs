using AdventureWorks.Models.ProductModels.Dtos;

namespace AdventureWorks.Services.ProductService
{
    public interface IProductService
    {
        public Task<List<ProductResponse>> GetProductsAsync(int page, int pageSize, string categoryName);

    }
}
