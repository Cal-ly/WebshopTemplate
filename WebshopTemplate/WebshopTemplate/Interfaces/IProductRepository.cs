namespace WebshopTemplate.Interfaces
{
    public interface IProductRepository : IRepository<Product>
    {
        Task<List<Product>?> GetByCategoryIdAsync(string categoryId);
    }
}
