using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Ciclilavarizia.Models.ProductModels.Dtos
{
    [BsonIgnoreExtraElements]
    public class ProductResponse
    {

        [BsonElement("ProductID")]
        public int ProductId { get; set; }

        [BsonElement("Name")]
        public string Name { get; set; } = null!;

        [BsonElement("ProductNumber")]
        public string ProductNumber { get; set; } = null!;

        [BsonElement("Color")]
        public string? Color { get; set; }

        [BsonElement("StandardCost")]
        public decimal StandardCost { get; set; }

        [BsonElement("ListPrice")]
        public decimal ListPrice { get; set; }

        [BsonElement("Size")]
        public string? Size { get; set; }

        [BsonElement("Weight")]
        public decimal? Weight { get; set; }

        [BsonElement("ProductCategoryID")]
        public int? ProductCategoryId { get; set; }

        [BsonElement("ProductModelID")]
        public int? ProductModelId { get; set; }

        public ProductCategoryResponse? ProductCategory { get; set; }

        public ProductModelResponse? ProductModel { get; set; }
    }
}
