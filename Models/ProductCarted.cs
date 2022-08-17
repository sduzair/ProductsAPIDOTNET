using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ProductsAPI.Models
{
    public class ProductCarted
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        [BsonElement("id")]
        public int SecondID { get; set; }
        [BsonElement("title")]
        public string Title { get; set; } = null!;
        public string Description { get; set; } = null!;
        public int Price { get; set; }
        public double DiscountPercentage { get; set; }
        public double Rating { get; set; }
        public string Brand { get; set; } = null!;
        public string Category { get; set; } = null!;
        public string Thumbnail { get; set; } = null!;
        public string[] Images { get; set; } = null!;
    }
}
