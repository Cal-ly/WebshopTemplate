namespace WebshopTemplate.Services.DTO
{
    public class ProductSalesDTO
    {
        public string? ProductId { get; set; }
        public string? ProductName { get; set; }
        public int TotalQuantitySold { get; set; }
        public decimal TotalSales { get; set; }
    }
}
