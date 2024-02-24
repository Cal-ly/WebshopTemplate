namespace WebshopTemplate.Services;

public class OrderService : IOrderService
{
    private readonly IOrderRepository orderRepository;

    public OrderService(IOrderRepository orderRepository)
    {
        this.orderRepository = orderRepository;
    }
    public async Task<Order?> CreateOrderFromBasketAsync(string userId, Basket basket)
    {
        var order = new Order // Implement: When a user submits an order and aren't logged in, the order should be created with a temporary user id, i.e. a placeholder Customer containing the Customer properties.
        {
            CustomerId = userId,
            OrderDate = DateTime.UtcNow,
            Status = OrderStatus.Submitted, // Set initial order status
            OrderDetails = new List<OrderDetail>(),
        };

        foreach (var item in basket.Items)
        {
            order.OrderDetails.Add(new OrderDetail
            {
                ProductId = item.ProductId,
                Quantity = item.Quantity,
                Price = item.ProductInBasket.Price,
            });
        }
        await (order != null ? orderRepository.Add(order) : throw new ArgumentNullException("Order is Null"));

        return order;
    }
    public async Task<List<Order>?> GetAllOrdersAsync()
    {
        return await orderRepository.GetAllAsync();
    }
    public async Task<Order?> UpdateOrderStatusAsync(string orderId, OrderStatus newStatus)
    {
        var order = await orderRepository.Get(orderId);
        if (order != null)
        {
            order.Status = newStatus;
            await orderRepository.UpdateAsync(order);
            return order;
        }
        throw new KeyNotFoundException("Order not found.");
    }
    public async Task<Order?> DeleteOrderAsync(string orderId)
    {
        var orderDelete = await orderRepository.Get(orderId);
        if (orderDelete != null)
        {
            return await orderRepository.DeleteAsync(orderId);
        }
        return orderDelete;
    }
    public async Task<Order?> GetOrderByIdAsync(string orderId)
    {
        return await orderRepository.Get(orderId);
    }
    public async Task<List<Order>?> GetOrdersByCustomerIdAsync(string customerId)
    {
        return await orderRepository.GetByCustomerIdAsync(customerId);
    }
}