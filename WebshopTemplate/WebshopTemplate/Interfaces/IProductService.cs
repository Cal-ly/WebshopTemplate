namespace WebshopTemplate.Interfaces
{
    public interface IProductService
    {
        Task<Product?> CreateProductAsync(Product product);
        Task<List<Product>> GetAllProductsAsync();
        Task<Product?> GetProductByIdAsync(string productId);
        Task<Product?> UpdateProductAsync(Product product);
        Task<Product?> DeleteProductAsync(string productId);
    }
}
