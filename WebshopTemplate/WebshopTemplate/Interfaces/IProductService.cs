namespace WebshopTemplate.Interfaces
{
    public interface IProductService
    {
        public Task<Product?> AddAsync(Product product);
        public Task<List<Product>?> GetAllAsync();
        public Task<Product?> UpdateAsync(Product product);
        public Task<Product?> DeleteAsync(string productId);
        public Task<Product?> GetProductByIdAsync(string productId);
    }
}
