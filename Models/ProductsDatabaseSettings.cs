namespace ProductsAPI.Models
{
    public class ProductDatabaseSettings
    {
        public string ConnectionString { get; set; } = null!;
        public string DatabaseName { get; set; } = null!;
        public string ProductsCollectionName { get; set; } = null!;
        public string CartsCollectionName { get; set; } = null!;
    }
}
