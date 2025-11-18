using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Ciclilavarizia.Models.ProductModels;

/// <summary>
/// High-level product categorization.
/// </summary>
public partial class ProductCategory
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string _id { get; set; } = string.Empty;

    [BsonElement("ProductCategoryID")]
    public int ProductCategoryId { get; set; }

    [BsonElement("ParentProductCategoryID")]
    public int? ParentProductCategoryId { get; set; }

    [BsonElement("Name")]
    public string Name { get; set; } = null!;

    [BsonElement("rowguid ")]
    public Guid Rowguid { get; set; }

    [BsonElement("ModifiedDate")]
    public string ModifiedDate { get; set; }
}
