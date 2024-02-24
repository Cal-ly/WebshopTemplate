namespace WebshopTemplate.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly ApplicationDbContext context;
        public OrderRepository(ApplicationDbContext context)
        {
            this.context = context;
        }
        public async Task<Order?> Add(Order order)
        {
            await context.Orders.AddAsync(order);
            await context.SaveChangesAsync();
            return order;
        }
        public async Task<Order?> Get(string id)
        {

            return await context.Orders
                .Include(o => o.OrderDetails)
                .ThenInclude(od => od.ProductInOrder)
                .FirstOrDefaultAsync(o => o.Id == id);
        }
        public async Task<List<Order>?> GetAllAsync()
        {
            return await context.Orders.AsNoTracking().ToListAsync();
        }
        public async Task<Order?> UpdateAsync(Order order)
        {
            context.Orders.Update(order);
            await context.SaveChangesAsync();
            return order;
        }
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
        public async Task<List<Order>?> GetOrdersByCustomerIdAsync(string customerId)
        {
            return await context.Orders
                .Where(o => o.CustomerId == customerId)
                .AsNoTracking()
                .ToListAsync();
        }
        public async Task<List<Order>?> GetOrdersByStaffIdAsync(string staffId)
        {
            return await context.Orders
                .Where(o => o.StaffId == staffId)
                .AsNoTracking()
                .ToListAsync();
        }
    }
}
