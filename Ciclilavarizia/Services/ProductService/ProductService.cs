using Ciclilavarizia.Data;
using Ciclilavarizia.Models.ProductModels.Dtos;
using MongoDB.Driver;

namespace Ciclilavarizia.Services.ProductService
{
    public class ProductService(MongoDbService mongoDbService) : IProductService
    {

        public async Task<List<ProductResponse>> GetAllProductsAsync()
        {
            return await mongoDbService.Products.Find(_ => true).ToListAsync();
        }
    }
}
