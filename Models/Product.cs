using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ProductsAPI.Models
{
    public class Product
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
        public int Stock { get; set; }
        public string Brand { get; set; } = null!;
        public string Category { get; set; } = null!;
        public string Thumbnail { get; set; } = null!;
        public string[] Images { get; set; } = null!;
        //public Carted? Carted { get; set; }
    }

    public class Carted
    {
        public int Qty { get; set; }
        [BsonRepresentation(BsonType.ObjectId)]
        public string Cart_id { get; set; } = null!;
        public DateTime TimeStamp { get; set; }
    }
}
