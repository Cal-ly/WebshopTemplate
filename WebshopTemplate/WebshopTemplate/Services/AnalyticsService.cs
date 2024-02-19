using WebshopTemplate.Models.DTO;

namespace WebshopTemplate.Services
{
    public class AnalyticsService : IAnalyticsService
    {
        private readonly ApplicationDbContext _context;
        public AnalyticsService(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<decimal> GetTotalSalesAsync(DateTime date)
        {
            decimal totalSales = 0;
            totalSales +=
                await _context.Orders
                    .Where(o => o.OrderDate == date.Date)
                        .SumAsync(o => o.OrderDetails!.Sum(od => od.Quantity * od.ProductInOrder.Price));
            return totalSales;
        }

        public async Task<decimal> GetTotalSalesFromToAsync(DateTime dateStart, DateTime dateEnd)
        {
            decimal totalSales = 0;
            totalSales +=
                await _context.Orders
                    .Where(o => o.OrderDate >= dateStart.Date && o.OrderDate <= dateEnd.Date)
                        .SumAsync(o => o.OrderDetails!.Sum(od => od.Quantity * od.ProductInOrder.Price));
            return totalSales;
        }

        public async Task<List<ProductSalesDTO>> GetTopSellingProductsAsync(DateTime startDate, DateTime endDate)
        {
            return await _context.OrderDetails
                .Where(od => od.Order.OrderDate >= startDate && od.Order.OrderDate <= endDate)
                .GroupBy(od => od.ProductId)
                .Select(group => new ProductSalesDTO
                {
                    ProductId = group.Key,
                    TotalQuantitySold = group.Sum(od => od.Quantity),
                    TotalSales = group.Sum(od => od.Quantity * od.ProductInOrder.Price)
                })
                .OrderByDescending(dto => dto.TotalSales)
                .ToListAsync();
        }
    }
}
