using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
//using MongoDB.Bson.Serialization.IdGenerators;

namespace ProductsAPI.Models
{
    public class Cart
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? SessionId { get; set; }
        public List<ProductCarted> CartItems { get; set; } = new List<ProductCarted>();
    }
}
