using MongoDB.Bson.Serialization.Attributes;

namespace AdventureWorks.Models.ProductModels.Dtos
{
    public class ProductModelResponse
    {
        [BsonElement("ProductModelID")]
        public int ProductModelId { get; set; }

        [BsonElement("Name")]
        public string Name { get; set; } = null!;

        [BsonElement("CatalogDescription")]
        public string? CatalogDescription { get; set; }
    }
}
