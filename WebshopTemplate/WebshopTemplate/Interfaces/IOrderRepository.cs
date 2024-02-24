namespace WebshopTemplate.Interfaces
{
    public interface IOrderRepository : IRepository<Order>
    {
        Task<List<Order>?> GetOrdersByCustomerIdAsync(string customerId);
        Task<List<Order>?> GetOrdersByStaffIdAsync(string staffId);
    }
}
