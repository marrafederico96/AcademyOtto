using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Ciclilavarizia.Models.ProductModels;

public partial class Product
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string _id { get; set; } = string.Empty;

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

    [BsonElement("SellStartDate")]
    [BsonRepresentation(BsonType.String)]
    public string SellStartDate { get; set; }

    [BsonElement("SellEndDate")]
    [BsonRepresentation(BsonType.String)]
    public string? SellEndDate { get; set; }

    [BsonElement("DiscontinuedDate")]
    [BsonRepresentation(BsonType.String)]
    public string? DiscontinuedDate { get; set; }

    [BsonElement("ThumbNailPhoto")]
    public string? ThumbNailPhoto { get; set; }

    [BsonElement("ThumbnailPhotoFileName")]
    public string? ThumbnailPhotoFileName { get; set; }

    [BsonElement("rowguid")]
    public Guid Rowguid { get; set; }

    [BsonElement("ModifiedDate")]
    [BsonRepresentation(BsonType.String)]
    public string ModifiedDate { get; set; }
}
