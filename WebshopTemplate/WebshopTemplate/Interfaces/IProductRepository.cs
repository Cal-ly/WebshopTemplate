namespace WebshopTemplate.Interfaces
{
    public interface IProductRepository
    {
        public Task<Product?> Add(Product product);
        public Task<Product?> Get(string id);
        public Task<List<Product>> GetAllAsync();
        public Task<Product?> UpdateProductAsync(Product product);
        public Task<Product?> DeleteProductAsync(string id);
    }
}
