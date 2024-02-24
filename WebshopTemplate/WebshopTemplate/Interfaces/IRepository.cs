namespace WebshopTemplate.Interfaces
{
    public interface IRepository<T> where T : class
    {
        Task<T?> Add(T entity);
        Task<T?> Get(string id);
        Task<List<T>?> GetAllAsync();
        Task<T?> UpdateAsync(T entity);
        Task<T?> DeleteAsync(string id);
    }
}