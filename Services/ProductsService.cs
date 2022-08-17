using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Driver;
using ProductsAPI.Models;

namespace ProductsAPI.Services;

public class ProductsService
{
    private readonly IMongoCollection<Product> _productsCollection;
    private readonly IMongoCollection<Cart> _cartsCollection;
    private const int DefaultLimit = 100;
    private const int DefaultSkip = 0;
    private const string DefaultSortKey = "id";
    private const int DefaultSortOrder = 1;

    public ProductsService(IOptions<ProductDatabaseSettings> productDatabaseSettings)
    {
        var camelCaseConvention = new ConventionPack { new CamelCaseElementNameConvention() };
        ConventionRegistry.Register("CamelCase", camelCaseConvention, type => true);
        var mongoClient = new MongoClient(productDatabaseSettings.Value.ConnectionString);
        var mongoDatabase = mongoClient.GetDatabase(productDatabaseSettings.Value.DatabaseName);
        _productsCollection = mongoDatabase.GetCollection<Product>(
            productDatabaseSettings.Value.ProductsCollectionName).WithWriteConcern(WriteConcern.WMajority);
        _cartsCollection = mongoDatabase.GetCollection<Cart>(productDatabaseSettings.Value.CartsCollectionName).WithReadConcern(ReadConcern.Majority);
    }
    //products methods
    public async Task<List<Product>> GetProductsAsync(int limit = DefaultLimit, string sort = DefaultSortKey, int sortDirection = DefaultSortOrder, int skip = DefaultSkip) 
    {
        var sortFilter = new BsonDocument(sort, sortDirection);
        return await _productsCollection
            .Find(_ => true)
            .Limit(limit)
            .Skip(skip)
            .Sort(sortFilter)
            .ToListAsync();
    }

    public async Task<Product> GetProductAsync(string id) => await _productsCollection.Find(x => x.Id == id).FirstOrDefaultAsync();
    public async Task<List<Product>> GetProductsByCategoryAsync(string category, int limit = DefaultLimit, string sort = DefaultSortKey, int sortDirection = DefaultSortOrder, int skip = DefaultSkip)
    {
        var sortFilter = new BsonDocument(sort, sortDirection);
        return await _productsCollection
            .Find(x => x.Category == category)
            .Limit(limit)
            .Skip(skip)
            .Sort(sortFilter)
            .ToListAsync();
    }


    public async Task CreateProductAsync(Product newProduct) => await _productsCollection.InsertOneAsync(newProduct);

    public async Task UpdateProductAsync(string id, Product updatedProduct) => await _productsCollection.ReplaceOneAsync(x => x.Id == id, updatedProduct);

    public async Task RemoveProductAsync(string id) => await _productsCollection.DeleteOneAsync(x => x.Id == id);

    // carts methods
    public async Task<List<Cart>> GetCartsAsync() => await _cartsCollection.Find(_ => true).ToListAsync();
    public async Task<Cart> GetCartAsync(string sessionId) => await _cartsCollection.Find(c => c.SessionId == sessionId).FirstOrDefaultAsync();
    public async Task<Cart> CreateCartAsync(Cart newCart)
    {
        await _cartsCollection.InsertOneAsync(newCart);
        return newCart;
    }
    public async Task<UpdateResult> UpdateCartAsync(string sessionId, ProductCarted p)
    {
        var update = Builders<Cart>.Update.Push(c => c.CartItems, p);
        var res = await _cartsCollection.UpdateOneAsync(c => c.SessionId == sessionId, update);
        return res;
    }
    public async Task RemoveCartAsync(string sessionId) => await _cartsCollection.DeleteOneAsync(c => c.SessionId == sessionId);

    public async Task<UpdateResult> RemoveAllProductsOfOneType(string sessionId, string productId)
    {
        var filterCarts = Builders<Cart>.Filter.Eq(c => c.SessionId, sessionId);
        //var filterProducts = Builders<ProductCarted>.Filter.
        var update = Builders<Cart>.Update.PullFilter(c => c.CartItems, p => p.Id == productId);
        var res = await _cartsCollection.UpdateOneAsync(filterCarts, update);
        return res;
    }

    [Obsolete]
    public async Task<UpdateResult> RemoveOneProduct(string sessionId, ProductCarted p)
    {

        var updateStage = new BsonDocument
        {
            new BsonDocument("$set",
            new BsonDocument("cartItems",
            new BsonDocument("$let",
            new BsonDocument
                        {
                            { "vars",
            new BsonDocument("ix",
            new BsonDocument("$indexOfArray",
            new BsonArray
                                    {
                                        "$cartItems.id",
                                        p.SecondID
                                    })) },
                            { "in",
            new BsonDocument("$cond",
            new BsonArray
                                {
                                    new BsonDocument("$eq",
                                    new BsonArray
                                        {
                                            "$$ix",
                                            0
                                        }),
                                    new BsonDocument("$slice",
                                    new BsonArray
                                        {
                                            "$cartItems",
                                            1,
                                            new BsonDocument("$size", "$cartItems")
                                        }),
                                    new BsonDocument("$concatArrays",
                                    new BsonArray
                                        {
                                            new BsonDocument("$slice",
                                            new BsonArray
                                                {
                                                    "$cartItems",
                                                    0,
                                                    "$$ix"
                                                }),
                                            new BsonArray(),
                                            new BsonDocument("$slice",
                                            new BsonArray
                                                {
                                                    "$cartItems",
                                                    new BsonDocument("$add",
                                                    new BsonArray
                                                        {
                                                            1,
                                                            "$$ix"
                                                        }),
                                                    new BsonDocument("$size", "$cartItems")
                                                })
                                        })
                                }) }
                        })))
        };
        var pipeline = PipelineDefinition<Cart, Cart>.Create(updateStage);
        var update = Builders<Cart>.Update.Pipeline(pipeline);


        var filter = Builders<Cart>.Filter.Eq(c => c.SessionId, sessionId);
        ////var update = Builders<Cart>.Update.PullFilter(c => c.CartItems, ci => ci.Id == p.Id);
        ////var update = Builders<Cart>.Update.PullFilter(c => c.CartItems, Builders<Cart>.Filter.Eq(c => c.CartItems[-1].Id, p.Id));
        var res = await _cartsCollection.UpdateOneAsync(filter, update);
        return res;
    }
}