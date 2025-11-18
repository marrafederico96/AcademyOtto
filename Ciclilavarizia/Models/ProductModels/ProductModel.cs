using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Ciclilavarizia.Models.ProductModels;

public partial class ProductModel
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string _id { get; set; } = string.Empty;

    [BsonElement("ProductModelID")]
    public int ProductModelId { get; set; }

    [BsonElement("Name")]
    public string Name { get; set; } = null!;

    [BsonElement("CatalogDescription")]
    public string? CatalogDescription { get; set; }

    [BsonElement("Rowguid")]
    public Guid Rowguid { get; set; }

    [BsonElement("ModifiedDate")]
    [BsonRepresentation(BsonType.String)]
    public DateTime ModifiedDate { get; set; }

}
