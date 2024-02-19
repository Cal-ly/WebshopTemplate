namespace WebshopTemplate.Services
{
    public interface IOrderService
    {
        Task<Order> CreateOrderFromBasketAsync(string userId, Basket basket);
        Task<Order> UpdateOrderStatusAsync(string orderId, OrderStatus newStatus);
        Task<Order?> GetOrderByIdAsync(string orderId);
        Task<List<Order>> GetOrdersByCustomerIdAsync(string customerId);
        Task<List<Order>> GetAllOrdersAsync();
        Task<Order?> DeleteOrderAsync(string orderId);
    }
}
