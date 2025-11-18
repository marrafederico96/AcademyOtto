using MongoDB.Driver;

namespace Ciclilavarizia.Services.ProductService
{
    public class ProductService(IMongoDatabase mongoDatabase) : IProductService
    {
    }
}
