namespace WebshopTemplate.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly ApplicationDbContext _context;

        public OrderRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task Add(Order order)
        {
            await _context.Orders.AddAsync(order);
            await _context.SaveChangesAsync();
        }

        public async Task<Order> Get(string id)
        {
            //return await _context.Orders
            //    .Include(o => o.OrderDetails)
            //    .ThenInclude(od => od.Product)
            //    .FirstOrDefaultAsync(o => o.Id == id);
            return await _context.Orders.FindAsync(id);
        }

        public async Task<List<Order>> GetAllAsync()
        {
            return await _context.Orders.AsNoTracking().ToListAsync();
        }

        public async Task<Order> UpdateOrderAsync(Order order)
        {
            _context.Orders.Update(order);
            await _context.SaveChangesAsync();
            return order;
        }

        public async Task<Order?> DeleteOrderAsync(string id)
        {
            var order = await _context.Orders.FindAsync(id);
            if (order != null)
            {
                _context.Orders.Remove(order);
                await _context.SaveChangesAsync();
            }
            return order == null ? order : throw new Exception("Order not found.");
        }

        public async Task<List<Order>> GetOrdersByCustomerIdAsync(string customerId)
        {
            return await _context.Orders
                .Where(o => o.CustomerId == customerId)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<List<Order>> GetOrdersByStaffIdAsync(string staffId)
        {
            return await _context.Orders
                .Where(o => o.StaffId == staffId)
                .AsNoTracking()
                .ToListAsync();
        }
    }
}
