using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace AdventureWorks.Models.ProductModels.Dtos
{
    [BsonIgnoreExtraElements]
    public class ProductCategoryResponse
    {
        [BsonElement("ProductCategoryID")]
        public int ProductCategoryId { get; set; }

        [BsonElement("Name")]

        public string Name { get; set; } = null!;

    }
}
