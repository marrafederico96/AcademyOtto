using Ciclilavarizia.Data;
using Ciclilavarizia.Models.ProductModels.Dtos;
using MongoDB.Driver;

namespace Ciclilavarizia.Services.ProductService
{
    public class ProductService(MongoDbService mongoDbService) : IProductService
    {

        public async Task<List<ProductResponse>> GetAllProductsAsync()
        {
            var products = await mongoDbService.Products
                .Aggregate()
                .Lookup(
                    foreignCollectionName: "ProductCategories",
                    localField: "ProductCategoryId",
                    foreignField: "ProductCategoryID",
                    @as: "ProductCategory")
                .Unwind("ProductCategory", new AggregateUnwindOptions<ProductResponse> { PreserveNullAndEmptyArrays = true })
                .As<ProductResponse>()
                .ToListAsync();

            return products;
        }
    }
}
