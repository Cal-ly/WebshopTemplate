namespace WebshopTemplate.Services
{
    public interface IOrderService
    {
        Task<Order?> CreateOrderFromBasketAsync(string userId, Basket basket);
        Task<List<Order>?> GetAllOrdersAsync();
        Task<Order?> UpdateOrderStatusAsync(string orderId, OrderStatus newStatus);
        Task<Order?> DeleteOrderAsync(string orderId);
        Task<Order?> GetOrderByIdAsync(string orderId);
        Task<List<Order>?> GetOrdersByCustomerIdAsync(string customerId);
    }
}
