namespace WebshopTemplate.Repositories
{
    public class ProductRepository
    {
        private readonly ApplicationDbContext _context;
        public ProductRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task Add(Product product)
        {
            await _context.Products.AddAsync(product);
            await _context.SaveChangesAsync();
        }
        public async Task<Product?> Get(string id)
        {
            return await _context.Products.FindAsync(id);
        }
        public async Task<List<Product>> GetAllAsync()
        {
            return await _context.Products.AsNoTracking().ToListAsync();
        }
        public async Task<Product> Update(Product product)
        {
            _context.Products.Update(product);
            await _context.SaveChangesAsync();
            return product;
        }
        public async Task<Product?> Delete(string id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product != null)
            {
                _context.Products.Remove(product);
                await _context.SaveChangesAsync();
            }
            return product == null ? product : throw new Exception("Product not found.");
        }
        public async Task<List<Product>> GetProductsByCategoryIdAsync(string categoryId)
        {
            return await _context.Products
                .Where(p => p.CategoryId == categoryId)
                .AsNoTracking()
                .ToListAsync();
        }
    }
}
