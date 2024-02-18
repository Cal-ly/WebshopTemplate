namespace WebshopTemplate.Interfaces
{
    public interface IOrderRepository
    {
        public Task Add(Order order);
        public Task<Order> Get(string id);
        public Task<List<Order>> GetAllAsync();
        public Task<Order> UpdateOrderAsync(Order order);
        public Task<Order?> DeleteOrderAsync(string id);
        public Task<List<Order>> GetOrdersByCustomerIdAsync(string customerId);
        public Task<List<Order>> GetOrdersByStaffIdAsync(string staffId);
    }
}
