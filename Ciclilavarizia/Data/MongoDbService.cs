using AdventureWorks.Models;
using AdventureWorks.Models.ProductModels;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace AdventureWorks.Data
{
    public class MongoDbService
    {
        private readonly IMongoDatabase _database;
        private readonly MongoDbSettings _settings;

        public MongoDbService(IOptions<MongoDbSettings> mongoDbSettings)
        {
            _settings = mongoDbSettings.Value ?? throw new ArgumentNullException(nameof(mongoDbSettings));
            var client = new MongoClient(_settings.ConnectionString);
            _database = client.GetDatabase(_settings.DatabaseName);
        }

        public IMongoCollection<Product> Products =>
                _database.GetCollection<Product>(_settings.Collections["Products"]);

        public IMongoCollection<ProductCategory> ProductCategories =>
            _database.GetCollection<ProductCategory>(_settings.Collections["ProductCategories"]);
    }
}
