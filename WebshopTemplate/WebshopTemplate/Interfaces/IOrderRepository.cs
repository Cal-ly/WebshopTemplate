namespace WebshopTemplate.Interfaces;

public interface IOrderRepository : IRepository<Order>
{
    /// <summary>
    /// Retrieves a list of orders by customer ID asynchronously.
    /// </summary>
    /// <param name="customerId">The customer ID.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the list of orders, or null if no orders are found.</returns>
    Task<List<Order>?> GetByCustomerIdAsync(string customerId);

    /// <summary>
    /// Retrieves a list of orders by staff ID asynchronously.
    /// </summary>
    /// <param name="staffId">The staff ID.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the list of orders, or null if no orders are found.</returns>
    Task<List<Order>?> GetByStaffIdAsync(string staffId);
}
