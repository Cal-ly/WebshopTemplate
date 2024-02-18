namespace WebshopTemplate.Interfaces
{
    public interface IOrderService
    {
        public Task<Order> CreateOrderAsync(Order order);
        public Task<Order> GetOrderAsync(int id);
        public Task<List<Order>> GetOrdersAsync();
        public Task<Order> UpdateOrderAsync(Order order);
        public Task<Order> DeleteOrderAsync(int id);
        public Task<Order> GetOrderByIdAsync(int id, string? include = null);
        public Task<List<Order>> GetOrdersByUserIdAsync(string userId);
    }
}
