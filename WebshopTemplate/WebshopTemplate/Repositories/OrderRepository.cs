namespace WebshopTemplate.Repositories;

public class OrderRepository : IOrderRepository
{
    private readonly ApplicationDbContext context;

    /// <summary>
    /// Initializes a new instance of the <see cref="OrderRepository"/> class.
    /// </summary>
    /// <param name="context">The application database context.</param>
    public OrderRepository(ApplicationDbContext context)
    {
        this.context = context;
    }

    /// <summary>
    /// Adds a new order to the database.
    /// </summary>
    /// <param name="order">The order to add.</param>
    /// <returns>The added order.</returns>
    public async Task<Order?> Add(Order order)
    {
        await context.Orders.AddAsync(order);
        await context.SaveChangesAsync();
        return order;
    }

    /// <summary>
    /// Retrieves an order by its ID.
    /// </summary>
    /// <param name="id">The ID of the order to retrieve.</param>
    /// <returns>The retrieved order, or null if not found.</returns>
    public async Task<Order?> Get(string id)
    {
        return await context.Orders
            .Include(o => o.OrderDetails)
            .ThenInclude(od => od.ProductInOrder)
            .FirstOrDefaultAsync(o => o.Id == id);
    }

    /// <summary>
    /// Retrieves all orders from the database.
    /// </summary>
    /// <returns>A list of orders</returns>
    public async Task<List<Order>?> GetAllAsync()
    {
        return await context.Orders.AsNoTracking().ToListAsync();
    }

    /// <summary>
    /// Updates an existing order in the database.
    /// </summary>
    /// <param name="order">The order to update.</param>
    /// <returns>The updated order.</returns>
    public async Task<Order?> UpdateAsync(Order order)
    {
        context.Orders.Update(order);
        await context.SaveChangesAsync();
        return order;
    }

    /// <summary>
    /// Deletes an order from the database.
    /// </summary>
    /// <param name="id">The ID of the order to delete.</param>
    /// <returns>The deleted order, or null if not found.</returns>
    public async Task<Order?> DeleteAsync(string id)
    {
        var order = await context.Orders.FindAsync(id);
        if (order != null)
        {
            context.Orders.Remove(order);
            await context.SaveChangesAsync();
        }
        return order == null ? order : throw new Exception("Order not found.");
    }

    /// <summary>
    /// Retrieves all orders associated with a customer.
    /// </summary>
    /// <param name="customerId">The ID of the customer.</param>
    /// <returns>A list of orders associated with the customer.</returns>
    public async Task<List<Order>?> GetByCustomerIdAsync(string customerId)
    {
        return await context.Orders
            .Where(o => o.CustomerId == customerId)
            .AsNoTracking()
            .ToListAsync();
    }

    /// <summary>
    /// Retrieves all orders associated with a staff member.
    /// </summary>
    /// <param name="staffId">The ID of the staff member.</param>
    /// <returns>A list of orders associated with the staff member.</returns>
    public async Task<List<Order>?> GetByStaffIdAsync(string staffId)
    {
        return await context.Orders
            .Where(o => o.StaffId == staffId)
            .AsNoTracking()
            .ToListAsync();
    }
}