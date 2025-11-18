using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Ciclilavarizia.Models.ProductModels.Dtos
{
    [BsonIgnoreExtraElements]
    public class ProductResponse
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string _id { get; set; } = string.Empty;

        public int ProductId { get; set; }

        public string Name { get; set; } = null!;

        public string ProductNumber { get; set; } = null!;

        public string? Color { get; set; }

        public decimal StandardCost { get; set; }

        public decimal ListPrice { get; set; }

        public string? Size { get; set; }

        public decimal? Weight { get; set; }

        public int? ProductCategoryId { get; set; }

        public int? ProductModelId { get; set; }
    }
}
