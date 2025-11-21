using Ciclilavarizia.Data;
using Ciclilavarizia.Models.ProductModels.Dtos;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Ciclilavarizia.Services.ProductService
{
    public class ProductService(MongoDbService mongoDbService) : IProductService
    {

        public async Task<List<ProductResponse>> GetProductsAsync(int page, int pageSize, string categoryName)
        {
            var aggregate = mongoDbService.Products
                           .Aggregate()
                           .Lookup(
                               foreignCollectionName: "ProductCategories",
                               localField: "ProductCategoryId",
                               foreignField: "ProductCategoryID",
                               @as: "ProductCategory")
                           .Unwind("ProductCategory");

            if (!String.Equals(categoryName, "All", StringComparison.InvariantCultureIgnoreCase))
            {
                aggregate = aggregate.Match(
                    new BsonDocument("ProductCategory.Name", categoryName)
                );
            }

            var products = await aggregate
                .As<ProductResponse>()
                .Skip((page - 1) * pageSize)
                .Limit(pageSize)
                .ToListAsync();

            return products;
        }

    }
}
