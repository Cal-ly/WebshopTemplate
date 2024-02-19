namespace WebshopTemplate.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;

        public ProductService(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<Product?> CreateProductAsync(Product product)
        {
            return await _productRepository.Add(product);
        }

        public async Task<List<Product>> GetAllProductsAsync()
        {
            return await _productRepository.GetAllAsync();
        }

        public async Task<Product?> GetProductByIdAsync(string productId)
        {
            return await _productRepository.Get(productId);
        }

        public async Task<Product?> UpdateProductAsync(Product product)
        {
            return await _productRepository.UpdateProductAsync(product);
        }

        public async Task<Product?> DeleteProductAsync(string productId)
        {
            return await _productRepository.DeleteProductAsync(productId);
        }
    }
}
