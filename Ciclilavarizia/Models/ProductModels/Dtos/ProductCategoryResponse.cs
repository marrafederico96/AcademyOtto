using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Ciclilavarizia.Models.ProductModels.Dtos
{
    [BsonIgnoreExtraElements]
    public class ProductCategoryResponse
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string _id { get; set; } = string.Empty;

        [BsonElement("ProductCategoryID")]
        public int ProductCategoryId { get; set; }

        [BsonElement("Name")]

        public string Name { get; set; } = null!;

    }
}
