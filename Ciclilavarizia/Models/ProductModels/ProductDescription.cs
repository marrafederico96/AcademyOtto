using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Ciclilavarizia.Models.ProductModels;

/// <summary>
/// Product descriptions in several languages.
/// </summary>
public partial class ProductDescription
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string _id { get; set; } = string.Empty;

    [BsonElement("ProductDescriptionID")]
    public int ProductDescriptionId { get; set; }

    [BsonElement("Description")]
    public string Description { get; set; } = null!;

    [BsonElement("rowguid")]
    public Guid Rowguid { get; set; }

    [BsonElement("ModifiedDate")]
    [BsonRepresentation(BsonType.String)]
    public DateTime ModifiedDate { get; set; }

}
