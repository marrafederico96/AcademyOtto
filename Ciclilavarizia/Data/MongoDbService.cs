using Ciclilavarizia.Models;
using Ciclilavarizia.Models.ProductModels.Dtos;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace Ciclilavarizia.Data
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

        public IMongoCollection<T> GetCollection<T>(string collectionName)
        {
            return _database.GetCollection<T>(collectionName);
        }

        public IMongoCollection<ProductResponse> Products
        {
            get
            {
                return _database.GetCollection<ProductResponse>(_settings.Collections["ProductCollection"]);
            }
        }
    }
}
