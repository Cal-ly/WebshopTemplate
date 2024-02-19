using System.Collections.Generic;
using System.Threading.Tasks;
using WebshopTemplate.Models;
using WebshopTemplate.Repositories;

namespace WebshopTemplate.Services
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly ApplicationDbContext _context;

        public OrderService(IOrderRepository orderRepository, ApplicationDbContext context)
        {
            _orderRepository = orderRepository;
            _context = context;
        }

        public async Task<Order> CreateOrderFromBasketAsync(string userId, Basket basket)
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
                    Price = item.Product.Price, // Assuming Price is a property of Product
                });
            }

            // Calculate the total price of the order if needed or perform additional processing
            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            return order;
        }

        public async Task<Order> UpdateOrderStatusAsync(string orderId, OrderStatus newStatus)
        {
            var order = await _orderRepository.Get(orderId);
            if (order != null)
            {
                order.Status = newStatus;
                await _orderRepository.UpdateOrderAsync(order);
                return order;
            }
            throw new KeyNotFoundException("Order not found.");
        }

        public async Task<Order?> GetOrderByIdAsync(string orderId)
        {
            return await _orderRepository.Get(orderId);
        }

        public async Task<List<Order>> GetOrdersByCustomerIdAsync(string customerId)
        {
            return await _orderRepository.GetOrdersByCustomerIdAsync(customerId);
        }

        public async Task<List<Order>> GetAllOrdersAsync()
        {
            return await _orderRepository.GetAllAsync();
        }

        public async Task<Order?> DeleteOrderAsync(string orderId)
        {
            var orderDelete = await _orderRepository.Get(orderId);
            if (orderDelete != null)
            {
                return await _orderRepository.DeleteOrderAsync(orderId);
            }
            return orderDelete;
        }
    }
}