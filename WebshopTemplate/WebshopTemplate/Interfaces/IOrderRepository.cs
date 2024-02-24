namespace WebshopTemplate.Interfaces;

public interface IOrderRepository : IRepository<Order>
{
    Task<List<Order>?> GetByCustomerIdAsync(string customerId);
    Task<List<Order>?> GetByStaffIdAsync(string staffId);
}
