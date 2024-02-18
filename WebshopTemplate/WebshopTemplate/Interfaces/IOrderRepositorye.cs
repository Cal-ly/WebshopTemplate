namespace WebshopTemplate.Interfaces
{
    public interface IOrderRepository
    {
        public void AddAsync(Order order);
        public Task<Order?> GetAsync(string id);
        public Task<List<Order?>> GetAllAsync();
        public Task<Order?> UpdateOrderAsync(Order order);
        public Task<Order?> DeleteOrderAsync(string id);
        public Task<Order?> GetOrderByIdAsync(string id, string? include = null);
        public Task<List<Order?>> GetOrdersByCustomerIdAsync(string customerId);
        public Task<List<Order?>> GetOrdersByStaffIdAsync(string staffId);
    }
}
