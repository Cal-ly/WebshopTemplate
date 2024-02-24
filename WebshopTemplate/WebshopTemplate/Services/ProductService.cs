namespace WebshopTemplate.Services;

public class ProductService : IProductService
{
    private readonly IProductRepository productRepository;
    public ProductService(IProductRepository productRepository)
    {
        this.productRepository = productRepository;
    }
    public async Task<Product?> AddAsync(Product product)
    {
        return await productRepository.Add(product);
    }
    public async Task<List<Product>?> GetAllAsync()
    {
        return await productRepository.GetAllAsync();
    }
    public async Task<Product?> UpdateAsync(Product product)
    {
        return await productRepository.UpdateAsync(product);
    }
    public async Task<Product?> DeleteAsync(string productId)
    {
        return await productRepository.DeleteAsync(productId);
    }
    public async Task<Product?> GetProductByIdAsync(string productId)
    {
        return await productRepository.Get(productId);
    }
}
