namespace WebshopTemplate.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly ApplicationDbContext context;
        public ProductRepository(ApplicationDbContext context)
        {
            this.context = context;
        }
        public async Task<Product?> Add(Product product)
        {
            await context.Products.AddAsync(product);
            await context.SaveChangesAsync();
            return product;
        }
        public async Task<Product?> Get(string id)
        {
            return await context.Products.FindAsync(id);
        }
        public async Task<List<Product>?> GetAllAsync()
        {

            return await context.Products.AsNoTracking().ToListAsync();
        }
        public async Task<Product?> UpdateAsync(Product product)
        {
            context.Products.Update(product);
            await context.SaveChangesAsync();
            return product;
        }
        public async Task<Product?> DeleteAsync(string id)
        {
            var product = await context.Products.FindAsync(id);
            if (product != null)
            {
                context.Products.Remove(product);
                await context.SaveChangesAsync();
            }
            return product == null ? product : throw new Exception("Product not found.");
        }
        public async Task<List<Product>?> GetByCategoryIdAsync(string categoryId)
        {
            return await context.Products
                .Where(p => p.CategoryId == categoryId)
                .AsNoTracking()
                .ToListAsync();
        }
    }
}
